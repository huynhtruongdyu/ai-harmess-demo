using AiCompany.Configuration;
using AiCompany.Memory;
using AiCompany.Tools;
using Microsoft.Extensions.Logging;

namespace AiCompany.Agents;

public class SystemAgent : IAgent
{
    private readonly IMemoryStore _memory;
    private readonly ToolRegistry _tools;
    private readonly ConfigLoader _config;
    private readonly ILogger<SystemAgent> _logger;

    public string Name => "system";

    public SystemAgent(
        IMemoryStore memory,
        ToolRegistry tools,
        ConfigLoader config,
        ILogger<SystemAgent> logger)
    {
        _memory = memory;
        _tools = tools;
        _config = config;
        _logger = logger;
    }

    public async Task<AgentResult> ExecuteAsync(AgentTask task, CancellationToken ct)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            return task.Action switch
            {
                "validate_file" => await ValidateFileAsync(task, ct),
                "list_workflows" => ListWorkflows(task),
                "list_agents" => ListAgents(task),
                "notify" => Notify(task),
                _ => new AgentResult(false, "", [], sw.Elapsed, 0, $"Unknown system action: {task.Action}")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "System agent failed on {Action}", task.Action);
            return new AgentResult(false, "", [], sw.Elapsed, 0, ex.Message);
        }
    }

    private async Task<AgentResult> ValidateFileAsync(AgentTask task, CancellationToken ct)
    {
        var path = task.Parameters?.GetValueOrDefault("path") ?? task.Input;
        var exists = await _memory.ExistsAsync(path, ct);
        return new AgentResult(true, exists ? "File exists" : "File not found", [], TimeSpan.Zero, 0);
    }

    private AgentResult ListWorkflows(AgentTask task)
    {
        var workflows = _config.ListWorkflows();
        return new AgentResult(true, string.Join("\n", workflows), [], TimeSpan.Zero, 0);
    }

    private AgentResult ListAgents(AgentTask task)
    {
        var agents = _config.ListAgents();
        return new AgentResult(true, string.Join("\n", agents), [], TimeSpan.Zero, 0);
    }

    private static AgentResult Notify(AgentTask task)
    {
        return new AgentResult(true, $"Notification sent: {task.Input}", [], TimeSpan.Zero, 0);
    }
}
