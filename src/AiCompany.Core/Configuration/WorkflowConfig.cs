using YamlDotNet.Serialization;

namespace AiCompany.Configuration;

public class WorkflowDefinition
{
    [YamlMember(Alias = "name")]
    public string Name { get; init; } = string.Empty;

    [YamlMember(Alias = "description")]
    public string Description { get; init; } = string.Empty;

    [YamlMember(Alias = "version")]
    public string? Version { get; init; }

    [YamlMember(Alias = "trigger")]
    public WorkflowTrigger Trigger { get; init; } = new();

    [YamlMember(Alias = "steps")]
    public List<WorkflowStep> Steps { get; init; } = new();

    [YamlMember(Alias = "error_handling")]
    public List<ErrorHandler>? ErrorHandling { get; init; }
}

public class WorkflowTrigger
{
    [YamlMember(Alias = "event")]
    public string Event { get; init; } = string.Empty;

    [YamlMember(Alias = "artifact")]
    public string? Artifact { get; init; }
}

public class WorkflowStep
{
    [YamlMember(Alias = "id")]
    public string Id { get; init; } = string.Empty;

    [YamlMember(Alias = "agent")]
    public string Agent { get; init; } = string.Empty;

    [YamlMember(Alias = "action")]
    public string Action { get; init; } = string.Empty;

    [YamlMember(Alias = "input")]
    public object? Input { get; init; }

    [YamlMember(Alias = "output")]
    public string? Output { get; init; }

    [YamlMember(Alias = "prompt_template")]
    public string? PromptTemplate { get; init; }

    [YamlMember(Alias = "template")]
    public string? Template { get; init; }

    [YamlMember(Alias = "parallel")]
    public bool Parallel { get; init; }

    [YamlMember(Alias = "target_agents")]
    public List<string>? TargetAgents { get; init; }

    [YamlMember(Alias = "gate")]
    public WorkflowGate? Gate { get; init; }

    [YamlMember(Alias = "environment")]
    public string? Environment { get; init; }

    [YamlMember(Alias = "timeout_minutes")]
    public int? TimeoutMinutes { get; init; }

    [YamlMember(Alias = "constraints")]
    public List<string>? Constraints { get; init; }

    [YamlMember(Alias = "mode")]
    public string? Mode { get; init; }

    [YamlMember(Alias = "strategy")]
    public object? Strategy { get; init; }

    [YamlMember(Alias = "canary")]
    public Dictionary<string, object>? Canary { get; init; }
}

public class WorkflowGate
{
    [YamlMember(Alias = "requires_approval")]
    public bool RequiresApproval { get; init; }

    [YamlMember(Alias = "approval_criteria")]
    public List<string>? ApprovalCriteria { get; init; }
}

public class ErrorHandler
{
    [YamlMember(Alias = "if")]
    public string Condition { get; init; } = string.Empty;

    [YamlMember(Alias = "action")]
    public string Action { get; init; } = string.Empty;

    [YamlMember(Alias = "to")]
    public string? Target { get; init; }

    [YamlMember(Alias = "message")]
    public string? Message { get; init; }
}
