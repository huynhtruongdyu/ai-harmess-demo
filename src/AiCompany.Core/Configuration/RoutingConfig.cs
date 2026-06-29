using YamlDotNet.Serialization;

namespace AiCompany.Configuration;

public class RoutingConfig
{
    [YamlMember(Alias = "routing")]
    public RoutingSettings Routing { get; init; } = new();
}

public class RoutingSettings
{
    [YamlMember(Alias = "default_strategy")]
    public string DefaultStrategy { get; init; } = "capability-match";

    [YamlMember(Alias = "rules")]
    public List<RoutingRule> Rules { get; init; } = new();

    [YamlMember(Alias = "fallback")]
    public RoutingFallback? Fallback { get; init; }
}

public class RoutingRule
{
    [YamlMember(Alias = "match")]
    public RoutingMatch Match { get; init; } = new();

    [YamlMember(Alias = "target_agent")]
    public string TargetAgent { get; init; } = string.Empty;

    [YamlMember(Alias = "priority")]
    public int Priority { get; init; }
}

public class RoutingMatch
{
    [YamlMember(Alias = "task_type")]
    public string? TaskType { get; init; }

    [YamlMember(Alias = "keywords")]
    public List<string>? Keywords { get; init; }
}

public class RoutingFallback
{
    [YamlMember(Alias = "action")]
    public string Action { get; init; } = "broadcast_to_all";

    [YamlMember(Alias = "timeout_seconds")]
    public int TimeoutSeconds { get; init; } = 120;
}
