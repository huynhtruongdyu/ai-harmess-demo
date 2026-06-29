using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiCompany.Integrations.LLM;

public class OpenAIProvider : ILLMProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _endpoint;

    public string Name => "openai";

    public OpenAIProvider(string apiKey, string? endpoint = null)
    {
        _apiKey = apiKey;
        _endpoint = endpoint ?? "https://api.openai.com/v1";
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<LLMResponse> CompleteAsync(LLMRequest request, CancellationToken ct = default)
    {
        var payload = new
        {
            model = request.Model ?? "gpt-4o",
            messages = BuildMessages(request),
            temperature = request.Temperature ?? 0.3,
            max_tokens = request.MaxTokens ?? 4000
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_endpoint}/chat/completions", content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var result = JsonSerializer.Deserialize<OpenAIResponse>(responseJson);

        if (result?.Choices == null || result.Choices.Length == 0)
            throw new InvalidOperationException("OpenAI returned no choices.");

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

    private static List<object> BuildMessages(LLMRequest request)
    {
        var messages = new List<object>
        {
            new { role = "system", content = request.SystemPrompt }
        };

        if (request.MessageHistory != null)
        {
            foreach (var msg in request.MessageHistory)
                messages.Add(new { role = msg.Role, content = msg.Content });
        }

        messages.Add(new { role = "user", content = request.UserPrompt });
        return messages;
    }

    public void Dispose() => _httpClient.Dispose();
}

internal class OpenAIResponse
{
    [JsonPropertyName("choices")]
    public Choice[]? Choices { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("usage")]
    public Usage? Usage { get; set; }
}

internal class Choice
{
    [JsonPropertyName("message")]
    public ResponseMessage? Message { get; set; }
}

internal class ResponseMessage
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("tool_calls")]
    public ResponseToolCall[]? ToolCalls { get; set; }
}

internal class ResponseToolCall
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("function")]
    public ResponseFunction? Function { get; set; }
}

internal class ResponseFunction
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("arguments")]
    public string? Arguments { get; set; }
}

internal class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}
