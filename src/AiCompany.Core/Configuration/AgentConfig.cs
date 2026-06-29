using YamlDotNet.Serialization;

namespace AiCompany.Configuration;

public class AgentConfig
{
    [YamlMember(Alias = "agent")]
    public AgentSettings Agent { get; init; } = new();

    [YamlMember(Alias = "memory_access")]
    public List<string> MemoryAccess { get; init; } = new();

    [YamlMember(Alias = "memory_write")]
    public List<string> MemoryWrite { get; init; } = new();

    [YamlMember(Alias = "tools")]
    public List<string> Tools { get; init; } = new();

    [YamlMember(Alias = "notification_channels")]
    public List<NotificationChannel>? NotificationChannels { get; init; }

    [YamlMember(Alias = "timeout_minutes")]
    public int TimeoutMinutes { get; init; } = 20;

    [YamlMember(Alias = "review_required")]
    public bool ReviewRequired { get; init; }
}

public class AgentSettings
{
    [YamlMember(Alias = "name")]
    public string Name { get; init; } = string.Empty;

    [YamlMember(Alias = "model")]
    public string Model { get; init; } = "gpt-4o";

    [YamlMember(Alias = "temperature")]
    public double Temperature { get; init; } = 0.3;

    [YamlMember(Alias = "max_tokens")]
    public int MaxTokens { get; init; } = 4000;
}

public class NotificationChannel
{
    [YamlMember(Alias = "type")]
    public string Type { get; init; } = "slack";

    [YamlMember(Alias = "channel")]
    public string? Channel { get; init; }

    [YamlMember(Alias = "frequency")]
    public string? Frequency { get; init; }
}
