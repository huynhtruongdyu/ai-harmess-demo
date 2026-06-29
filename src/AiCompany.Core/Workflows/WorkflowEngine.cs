using System.Collections.Concurrent;
using System.Diagnostics;
using AiCompany.Agents;
using AiCompany.Configuration;
using Microsoft.Extensions.Logging;

namespace AiCompany.Workflows;

public class WorkflowEngine
{
    private readonly AgentRegistry _agentRegistry;
    private readonly ConfigLoader _configLoader;
    private readonly RuntimeStore _runtimeStore;
    private readonly ILogger<WorkflowEngine> _logger;
    private readonly ConcurrentDictionary<string, WorkflowRun> _activeRuns = new();

    public WorkflowEngine(
        AgentRegistry agentRegistry,
        ConfigLoader configLoader,
        RuntimeStore runtimeStore,
        ILogger<WorkflowEngine> logger)
    {
        _agentRegistry = agentRegistry;
        _configLoader = configLoader;
        _runtimeStore = runtimeStore;
        _logger = logger;
    }

    public async Task<WorkflowRun> ExecuteAsync(
        string workflowName,
        Dictionary<string, string>? inputs = null,
        CancellationToken ct = default,
        Action<WorkflowStepProgress>? onStepProgress = null)
    {
        var definition = _configLoader.LoadWorkflow(workflowName);
        var runId = $"wf-{workflowName}-{DateTime.UtcNow:yyyyMMdd-HHmmss}";
        var sw = Stopwatch.StartNew();

        _logger.LogInformation("Starting workflow {Workflow} (Run: {RunId})", workflowName, runId);

        var run = new WorkflowRun(runId, workflowName, WorkflowStatus.Running, [], DateTime.UtcNow, null, null);
        _activeRuns.TryAdd(runId, run);
        _runtimeStore.SaveRun(run);

        try
        {
            var results = new List<WorkflowStepResult>();
            var totalSteps = definition.Steps.Count;

            for (var i = 0; i < totalSteps; i++)
            {
                ct.ThrowIfCancellationRequested();

                var step = definition.Steps[i];

                _logger.LogInformation("Executing step {StepId} with agent {Agent}", step.Id, step.Agent);

                onStepProgress?.Invoke(new WorkflowStepProgress(
                    i + 1, totalSteps, step.Id, step.Agent, step.Action,
                    StepStatus.Running, null, null, step.Gate?.RequiresApproval == true));

                var stepResult = await ExecuteStepAsync(step, runId, inputs, ct);
                results.Add(stepResult);
                _runtimeStore.SaveRun(run with { Steps = results.AsReadOnly() });

                onStepProgress?.Invoke(new WorkflowStepProgress(
                    i + 1, totalSteps, step.Id, step.Agent, step.Action,
                    stepResult.Status, stepResult.Duration, stepResult.Error,
                    step.Gate?.RequiresApproval == true));

                if (stepResult.Status == StepStatus.Rejected)
                {
                    _logger.LogWarning("Step {StepId} rejected. Workflow paused.", step.Id);
                    run = run with
                    {
                        Status = WorkflowStatus.PendingApproval,
                        Steps = results.AsReadOnly()
                    };
                    _activeRuns[runId] = run;
                    _runtimeStore.SaveRun(run);
                    return run;
                }

                if (!stepResult.Success && definition.ErrorHandling?.Any() == true)
                {
                    var handler = definition.ErrorHandling.Find(h =>
                        h.Condition.Contains(stepResult.Error ?? "", StringComparison.OrdinalIgnoreCase));

                    if (handler != null)
                    {
                        _logger.LogWarning("Error handler triggered: {Action} - {Message}",
                            handler.Action, handler.Message);
                        results.Add(new WorkflowStepResult(
                            "error-handler", handler.Action, StepStatus.Skipped, true,
                            handler.Message ?? "", null, TimeSpan.Zero));
                    }
                }
            }

            sw.Stop();
            var completedRun = run with
            {
                Status = results.All(r => r.Success) ? WorkflowStatus.Completed : WorkflowStatus.Failed,
                Steps = results.AsReadOnly(),
                CompletedAt = DateTime.UtcNow
            };
                    _activeRuns[runId] = completedRun;

            _runtimeStore.SaveRun(completedRun);
            _logger.LogInformation("Workflow {Workflow} completed in {Duration}",
                workflowName, sw.Elapsed);
            return completedRun;
        }
        catch (Exception ex)
        {
            sw.Stop();
            var failedRun = run with
            {
                Status = WorkflowStatus.Failed,
                Error = ex.Message,
                CompletedAt = DateTime.UtcNow
            };
            _activeRuns[runId] = failedRun;
            _runtimeStore.SaveRun(failedRun);
            _logger.LogError(ex, "Workflow {Workflow} failed", workflowName);
            return failedRun;
        }
    }

    private async Task<WorkflowStepResult> ExecuteStepAsync(
        WorkflowStep step, string runId,
        Dictionary<string, string>? inputs,
        CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            if (step.Agent == "system")
            {
                await Task.Delay(100, ct);
                sw.Stop();
                return new WorkflowStepResult(step.Id, step.Agent, StepStatus.Completed, true,
                    "System step executed", null, sw.Elapsed);
            }

            if (step.Parallel && step.TargetAgents?.Count > 0)
            {
                var tasks = step.TargetAgents.Select(agentName =>
                    ExecuteAgentStepAsync(agentName, step, runId, inputs, ct));
                var parallelResults = await Task.WhenAll(tasks);

                sw.Stop();
                var allSuccess = parallelResults.All(r => r.Success);
                return new WorkflowStepResult(step.Id, step.Agent, StepStatus.Completed, allSuccess,
                    string.Join("; ", parallelResults.Select(r => r.Output)),
                    allSuccess ? null : "Some parallel tasks failed", sw.Elapsed);
            }

            var result = await ExecuteAgentStepAsync(step.Agent, step, runId, inputs, ct);
            sw.Stop();

            var stepStatus = (step.Gate?.RequiresApproval == true && result.Success)
                ? StepStatus.PendingApproval
                : result.Success ? StepStatus.Completed : StepStatus.Failed;

            return new WorkflowStepResult(
                step.Id, step.Agent, stepStatus, result.Success,
                result.Output, result.Error, sw.Elapsed);
        }
        catch (Exception ex)
        {
            sw.Stop();
            return new WorkflowStepResult(step.Id, step.Agent, StepStatus.Failed, false, "", ex.Message, sw.Elapsed);
        }
    }

    private async Task<AgentResult> ExecuteAgentStepAsync(
        string agentName, WorkflowStep step, string runId,
        Dictionary<string, string>? inputs,
        CancellationToken ct)
    {
        var agent = _agentRegistry.Get(agentName);
        var agentTask = new AgentTask(
            Id: $"{runId}-{step.Id}",
            AgentName: agentName,
            Action: step.Action,
            Input: step.Input?.ToString() ?? "",
            Parameters: inputs
        );
        return await agent.ExecuteAsync(agentTask, ct);
    }

    public WorkflowRun? GetStatus(string runId) =>
        _activeRuns.TryGetValue(runId, out var run) ? run : null;

    public IReadOnlyCollection<string> GetActiveRuns() =>
        _activeRuns.Where(kv => kv.Value.Status == WorkflowStatus.Running)
            .Select(kv => kv.Key).ToList().AsReadOnly();
}

public record WorkflowRun(
    string Id,
    string WorkflowName,
    WorkflowStatus Status,
    IReadOnlyList<WorkflowStepResult> Steps,
    DateTime StartedAt,
    DateTime? CompletedAt,
    string? Error
);

public record WorkflowStepResult(
    string StepId,
    string Agent,
    StepStatus Status,
    bool Success,
    string Output,
    string? Error,
    TimeSpan Duration
);

public enum WorkflowStatus
{
    Running,
    PendingApproval,
    Completed,
    Failed
}

public enum StepStatus
{
    Pending,
    Running,
    Completed,
    PendingApproval,
    Rejected,
    Skipped,
    Failed
}

public record WorkflowStepProgress(
    int StepIndex,
    int TotalSteps,
    string StepId,
    string Agent,
    string Action,
    StepStatus Status,
    TimeSpan? Duration,
    string? Error,
    bool RequiresApproval
);
