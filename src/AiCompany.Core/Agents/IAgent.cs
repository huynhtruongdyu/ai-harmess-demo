namespace AiCompany.Agents;

public interface IAgent
{
    string Name { get; }
    Task<AgentResult> ExecuteAsync(AgentTask task, CancellationToken ct = default);
}

public record AgentTask(
    string Id,
    string AgentName,
    string Action,
    string Input,
    string[]? ContextFiles = null,
    Dictionary<string, string>? Parameters = null
);

public record AgentResult(
    bool Success,
    string Output,
    string[] Artifacts,
    TimeSpan Duration,
    int TokensUsed,
    string? Error = null
);
