using System.Diagnostics;
using AiCompany.Configuration;
using AiCompany.Memory;
using AiCompany.Tools;
using Microsoft.Extensions.Logging;

namespace AiCompany.Agents;

public class AgentRuntime : IAgent
{
    private readonly AgentConfig _config;
    private readonly IMemoryStore _memory;
    private readonly ToolRegistry _toolRegistry;
    private readonly ILogger<AgentRuntime> _logger;
    private readonly string _agentDir;

    public string Name => _config.Agent.Name;

    public Func<string, string, string?, double?, int?, CancellationToken, Task<string>>? LlmProcessor { get; set; }

    public AgentRuntime(
        AgentConfig config,
        IMemoryStore memory,
        ToolRegistry toolRegistry,
        ILogger<AgentRuntime> logger)
    {
        _config = config;
        _memory = memory;
        _toolRegistry = toolRegistry;
        _logger = logger;
        _agentDir = Path.Combine(Directory.GetCurrentDirectory(), ".ai", "agents", _config.Agent.Name);
    }

    public async Task<AgentResult> ExecuteAsync(AgentTask task, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        _logger.LogInformation("Agent {Agent} executing task {TaskId}: {Action}",
            Name, task.Id, task.Action);

        try
        {
            var systemPrompt = await LoadFileAsync("system.md", ct);
            var rules = await LoadFileAsync("rules.md", ct);
            var examples = await LoadFileAsync("examples.md", ct);

            var memoryContext = await BuildMemoryContextAsync(ct);

            var prompt = BuildPrompt(systemPrompt, rules, examples, memoryContext, task);

            _logger.LogInformation("Agent {Agent} built prompt ({Len} chars)", Name, prompt.Length);

            var result = await ProcessWithLLM(prompt, ct);

            sw.Stop();
            _logger.LogInformation("Agent {Agent} completed task {TaskId} in {Duration}",
                Name, task.Id, sw.Elapsed);

            return new AgentResult(
                true,
                result,
                new[] { $".ai/memory/current/tasks/{task.Id}.md" },
                sw.Elapsed,
                result.Length / 4,
                null
            );
        }
        catch (OperationCanceledException)
        {
            return new AgentResult(false, "", Array.Empty<string>(), sw.Elapsed, 0, "Task cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Agent {Agent} failed on task {TaskId}", Name, task.Id);
            return new AgentResult(false, "", Array.Empty<string>(), sw.Elapsed, 0, ex.Message);
        }
    }

    private async Task<string> LoadFileAsync(string filename, CancellationToken ct)
    {
        var path = Path.Combine(_agentDir, filename);
        if (File.Exists(path))
            return await File.ReadAllTextAsync(path, ct);
        return "";
    }

    private async Task<string> BuildMemoryContextAsync(CancellationToken ct)
    {
        var parts = new List<string>();
        foreach (var store in _config.MemoryAccess)
        {
            var content = await _memory.ReadAsync(store, ct);
            if (!string.IsNullOrEmpty(content))
                parts.Add($"=== {store} ===\n{content}");
        }
        return string.Join("\n\n", parts);
    }

    private static string BuildPrompt(
        string systemPrompt, string rules, string examples,
        string memoryContext, AgentTask task)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine(systemPrompt);
        sb.AppendLine();
        if (!string.IsNullOrEmpty(rules))
        {
            sb.AppendLine("## Rules");
            sb.AppendLine(rules);
            sb.AppendLine();
        }
        if (!string.IsNullOrEmpty(examples))
        {
            sb.AppendLine("## Examples");
            sb.AppendLine(examples);
            sb.AppendLine();
        }
        sb.AppendLine("## Context");
        sb.AppendLine(memoryContext);
        sb.AppendLine();
        sb.AppendLine("## Task");
        sb.AppendLine($"Action: {task.Action}");
        sb.AppendLine($"Input: {task.Input}");
        if (task.Parameters != null)
        {
            foreach (var kv in task.Parameters)
                sb.AppendLine($"{kv.Key}: {kv.Value}");
        }
        sb.AppendLine();
        sb.AppendLine("## Output");
        sb.AppendLine("Provide your response below.");

        return sb.ToString();
    }

    private async Task<string> ProcessWithLLM(string prompt, CancellationToken ct)
    {
        if (LlmProcessor != null)
        {
            var systemPrompt = prompt;
            var userPrompt = "";
            var idx = prompt.IndexOf("## Task", StringComparison.Ordinal);
            if (idx >= 0)
            {
                systemPrompt = prompt[..idx];
                userPrompt = prompt[idx..];
            }

            return await LlmProcessor(
                systemPrompt, userPrompt,
                _config.Agent.Model,
                _config.Agent.Temperature,
                _config.Agent.MaxTokens,
                ct);
        }

        _logger.LogWarning("Agent {Agent} running in standalone mode (no LLM provider configured)", Name);
        return $"[{Name} processed task successfully. Prompt length: {prompt.Length} chars.]";
    }
}
