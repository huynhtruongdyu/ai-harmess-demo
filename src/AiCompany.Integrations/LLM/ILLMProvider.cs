namespace AiCompany.Integrations.LLM;

public interface ILLMProvider
{
    string Name { get; }
    Task<LLMResponse> CompleteAsync(LLMRequest request, CancellationToken ct = default);
}

public record LLMRequest(
    string SystemPrompt,
    string UserPrompt,
    string? Model = null,
    double? Temperature = null,
    int? MaxTokens = null,
    IReadOnlyList<ChatMessage>? MessageHistory = null
);

public record LLMResponse(
    string Content,
    string Model,
    int PromptTokens,
    int CompletionTokens,
    IReadOnlyList<ToolCall>? ToolCalls = null
);

public record ChatMessage(string Role, string Content);

public record ToolCall(string Id, string FunctionName, string Arguments);
