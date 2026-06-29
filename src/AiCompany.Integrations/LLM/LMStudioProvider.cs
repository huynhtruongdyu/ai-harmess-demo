using System.Net;
using System.Text;
using System.Text.Json;

namespace AiCompany.Integrations.LLM;

public class LMStudioProvider : ILLMProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;
    private readonly int _maxRetries;
    private readonly int _baseDelaySeconds;
    private readonly int _contextLength;
    private readonly int _reservedForOutput;

    public string Name => "lmstudio";

    public LMStudioProvider(string apiKey, string? endpoint = null,
        int maxRetries = 3, int baseDelaySeconds = 30, int contextLength = 4096)
    {
        _endpoint = endpoint ?? "http://localhost:1234/v1";
        _maxRetries = maxRetries;
        _baseDelaySeconds = baseDelaySeconds;
        _contextLength = contextLength;
        _reservedForOutput = contextLength / 4;
        _httpClient = new HttpClient();
    }

    public async Task<LLMResponse> CompleteAsync(LLMRequest request, CancellationToken ct = default)
    {
        var systemPrompt = request.SystemPrompt;
        var messageHistory = request.MessageHistory;
        var userPrompt = request.UserPrompt;

        var estimatedTotal = EstimateTokens(systemPrompt, userPrompt, messageHistory);
        var maxInputTokens = _contextLength - _reservedForOutput;

        if (estimatedTotal > maxInputTokens)
        {
            systemPrompt = await SummarizeOverflowAsync(
                systemPrompt, userPrompt, messageHistory, maxInputTokens, ct);
        }

        var payload = BuildPayload(systemPrompt, userPrompt, request.Model,
            request.Temperature, request.MaxTokens, messageHistory);
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var attempt = 0;
        while (true)
        {
            ct.ThrowIfCancellationRequested();

            var response = await _httpClient.PostAsync($"{_endpoint}/chat/completions", content, ct);

            if (response.IsSuccessStatusCode)
                return ParseResponse(await response.Content.ReadAsStringAsync(ct));

            var body = await response.Content.ReadAsStringAsync(ct);

            if (response.StatusCode != HttpStatusCode.TooManyRequests
                && (int)response.StatusCode < 500)
            {
                throw new HttpRequestException(
                    $"LMStudio API returned {(int)response.StatusCode}: {body}");
            }

            if (attempt >= _maxRetries)
            {
                throw new HttpRequestException(
                    $"LMStudio API returned {(int)response.StatusCode} after {_maxRetries} retries: {body}");
            }

            attempt++;

            var delay = response.Headers.RetryAfter?.Delta?.TotalSeconds is double retryAfter
                ? retryAfter
                : _baseDelaySeconds * Math.Pow(2, attempt - 1);

            delay = Math.Min(delay, 120);
            await Task.Delay(TimeSpan.FromSeconds(delay), ct);
        }
    }

    private async Task<string> SummarizeOverflowAsync(
        string systemPrompt, string userPrompt,
        IReadOnlyList<ChatMessage>? messageHistory,
        int maxInputTokens, CancellationToken ct)
    {
        var overflowChars = EstimateTokens(systemPrompt, userPrompt, messageHistory) - maxInputTokens;
        overflowChars = overflowChars * 4 + 200;

        if (overflowChars >= systemPrompt.Length)
            overflowChars = systemPrompt.Length / 2;

        var overflowContent = systemPrompt[^Math.Min(overflowChars, systemPrompt.Length)..];

        var summaryPayload = new Dictionary<string, object>
        {
            ["messages"] = new List<object>
            {
                new { role = "system", content = "You are a precise text summarizer. Condense the following content into a concise summary, preserving all key instructions, requirements, constraints, and important details. Output only the summary." },
                new { role = "user", content = overflowContent }
            },
            ["temperature"] = 0.2,
            ["max_tokens"] = _reservedForOutput / 2
        };

        var json = JsonSerializer.Serialize(summaryPayload);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(
            $"{_endpoint}/chat/completions", httpContent, ct);

        var prefixLen = systemPrompt.Length - overflowChars;
        if (prefixLen < 0) prefixLen = 0;

        if (!response.IsSuccessStatusCode)
        {
            if (prefixLen < 100) prefixLen = 100;
            return systemPrompt[..prefixLen];
        }

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var result = JsonSerializer.Deserialize<OpenAIResponse>(responseJson);
        var summary = result?.Choices?.FirstOrDefault()?.Message?.Content ?? "";

        return prefixLen > 0
            ? systemPrompt[..prefixLen] + "\n\n[CONTEXT SUMMARY]\n" + summary
            : summary;
    }

    private static int EstimateTokens(string systemPrompt, string userPrompt,
        IReadOnlyList<ChatMessage>? messageHistory)
    {
        var total = systemPrompt.Length + userPrompt.Length;
        if (messageHistory != null)
        {
            foreach (var msg in messageHistory)
                total += msg.Content.Length;
        }
        return total / 4;
    }

    private static Dictionary<string, object> BuildPayload(
        string systemPrompt, string userPrompt, string? model,
        double? temperature, int? maxTokens,
        IReadOnlyList<ChatMessage>? messageHistory)
    {
        var messages = new List<object>
        {
            new { role = "system", content = systemPrompt }
        };

        if (messageHistory != null)
        {
            foreach (var msg in messageHistory)
                messages.Add(new { role = msg.Role, content = msg.Content });
        }

        messages.Add(new { role = "user", content = userPrompt });

        var payload = new Dictionary<string, object>
        {
            ["messages"] = messages,
            ["temperature"] = temperature ?? 0.3,
            ["max_tokens"] = maxTokens ?? 4000
        };

        if (!string.IsNullOrEmpty(model))
            payload["model"] = model;

        return payload;
    }

    private static LLMResponse ParseResponse(string responseJson)
    {
        var result = JsonSerializer.Deserialize<OpenAIResponse>(responseJson);

        if (result?.Choices == null || result.Choices.Length == 0)
            throw new InvalidOperationException("LMStudio returned no choices.");

        var choice = result.Choices[0];

        return new LLMResponse(
            choice.Message?.Content ?? "",
            result.Model ?? "unknown",
            result.Usage?.PromptTokens ?? 0,
            result.Usage?.CompletionTokens ?? 0,
            choice.Message?.ToolCalls?
                .Select(tc => new ToolCall(tc.Id ?? "", tc.Function?.Name ?? "", tc.Function?.Arguments ?? ""))
                .ToList()
                .AsReadOnly()
        );
    }

    public void Dispose() => _httpClient.Dispose();
}
