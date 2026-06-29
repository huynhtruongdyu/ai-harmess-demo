using System.Net;
using System.Text;
using System.Text.Json;

namespace AiCompany.Integrations.LLM;

public class GeminiProvider : ILLMProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _endpoint;
    private readonly int _maxRetries;
    private readonly int _baseDelaySeconds;

    public string Name => "gemini";

    public GeminiProvider(string apiKey, string? endpoint = null,
        int maxRetries = 3, int baseDelaySeconds = 30)
    {
        _apiKey = apiKey;
        _endpoint = (endpoint ?? "https://generativelanguage.googleapis.com").TrimEnd('/');
        _maxRetries = maxRetries;
        _baseDelaySeconds = baseDelaySeconds;
        _httpClient = new HttpClient();
    }

    public async Task<LLMResponse> CompleteAsync(LLMRequest request, CancellationToken ct = default)
    {
        var model = request.Model ?? "gemini-3.5-flash";
        var url = $"{_endpoint}/v1beta/models/{model}:generateContent";

        var payload = BuildPayload(request);
        var json = JsonSerializer.Serialize(payload);

        var attempt = 0;
        while (true)
        {
            ct.ThrowIfCancellationRequested();

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("x-goog-api-key", _apiKey);
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest, ct);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync(ct);
                return ParseResponse(responseJson, model);
            }

            if (response.StatusCode != HttpStatusCode.TooManyRequests
                && (int)response.StatusCode < 500)
            {
                response.EnsureSuccessStatusCode();
            }

            if (attempt >= _maxRetries)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException(
                    $"Gemini API returned {(int)response.StatusCode} after {_maxRetries} retries: {body}");
            }

            attempt++;

            var delay = response.Headers.RetryAfter?.Delta?.TotalSeconds is double retryAfter
                ? retryAfter
                : _baseDelaySeconds * Math.Pow(2, attempt - 1);

            delay = Math.Min(delay, 120);
            await Task.Delay(TimeSpan.FromSeconds(delay), ct);
        }
    }

    private static Dictionary<string, object> BuildPayload(LLMRequest request)
    {
        var body = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(request.SystemPrompt))
        {
            body["system_instruction"] = new Dictionary<string, object>
            {
                ["parts"] = new[] { new Dictionary<string, object> { ["text"] = request.SystemPrompt } }
            };
        }

        var contents = new List<object>();

        if (request.MessageHistory != null)
        {
            foreach (var msg in request.MessageHistory)
            {
                var role = msg.Role switch
                {
                    "assistant" => "model",
                    "system" => "user",
                    _ => msg.Role
                };
                contents.Add(new Dictionary<string, object>
                {
                    ["role"] = role,
                    ["parts"] = new[] { new Dictionary<string, object> { ["text"] = msg.Content } }
                });
            }
        }

        contents.Add(new Dictionary<string, object>
        {
            ["role"] = "user",
            ["parts"] = new[] { new Dictionary<string, object> { ["text"] = request.UserPrompt } }
        });

        body["contents"] = contents;

        var genConfig = new Dictionary<string, object>();
        if (request.Temperature.HasValue)
            genConfig["temperature"] = request.Temperature.Value;
        if (request.MaxTokens.HasValue)
            genConfig["maxOutputTokens"] = request.MaxTokens.Value;
        if (genConfig.Count > 0)
            body["generationConfig"] = genConfig;

        return body;
    }

    private static LLMResponse ParseResponse(string json, string model)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
            throw new InvalidOperationException("Gemini returned no candidates.");

        var content = candidates[0].GetProperty("content");
        var parts = content.GetProperty("parts");
        var text = parts.GetArrayLength() > 0
            ? parts[0].GetProperty("text").GetString() ?? ""
            : "";

        var promptTokens = 0;
        var completionTokens = 0;
        if (root.TryGetProperty("usageMetadata", out var usage))
        {
            if (usage.TryGetProperty("promptTokenCount", out var pt))
                promptTokens = pt.GetInt32();
            if (usage.TryGetProperty("candidatesTokenCount", out var ct))
                completionTokens = ct.GetInt32();
        }

        return new LLMResponse(text, model, promptTokens, completionTokens);
    }

    public void Dispose() => _httpClient.Dispose();
}
