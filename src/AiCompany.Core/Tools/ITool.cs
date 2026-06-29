namespace AiCompany.Tools;

public interface ITool
{
    string Name { get; }
    Task<ToolResult> ExecuteAsync(ToolInput input, CancellationToken ct = default);
}

public record ToolInput(
    string Operation,
    Dictionary<string, object?> Parameters,
    ToolExecutionContext Context
);

public record ToolResult(
    bool Success,
    object? Data,
    string? Error,
    TimeSpan Duration
);

public record ToolExecutionContext(
    string AgentName,
    string? TaskId,
    Func<string, CancellationToken, Task<string>> ReadMemoryAsync,
    Func<string, CancellationToken, Task<bool>> WriteMemoryAsync
);
