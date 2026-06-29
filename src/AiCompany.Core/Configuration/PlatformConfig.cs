using YamlDotNet.Serialization;

namespace AiCompany.Configuration;

public class PlatformConfig
{
    [YamlMember(Alias = "platform")]
    public PlatformSettings Platform { get; init; } = new();

    [YamlMember(Alias = "providers")]
    public Dictionary<string, ProviderConfig> Providers { get; init; } = new();
}

public class PlatformSettings
{
    [YamlMember(Alias = "name")]
    public string Name { get; init; } = "AiCompany";

    [YamlMember(Alias = "version")]
    public string Version { get; init; } = "1.0.0";

    [YamlMember(Alias = "default_provider")]
    public string DefaultProvider { get; init; } = "openai";

    [YamlMember(Alias = "default_model")]
    public string DefaultModel { get; init; } = "gpt-4o";

    [YamlMember(Alias = "execution")]
    public ExecutionConfig Execution { get; init; } = new();

    [YamlMember(Alias = "memory")]
    public MemoryConfig Memory { get; init; } = new();

    [YamlMember(Alias = "logging")]
    public LoggingConfig Logging { get; init; } = new();

    [YamlMember(Alias = "security")]
    public SecurityConfig Security { get; init; } = new();
}

public class ExecutionConfig
{
    [YamlMember(Alias = "max_concurrent_agents")]
    public int MaxConcurrentAgents { get; init; } = 5;

    [YamlMember(Alias = "workflow_timeout_seconds")]
    public int WorkflowTimeoutSeconds { get; init; } = 3600;

    [YamlMember(Alias = "agent_timeout_seconds")]
    public int AgentTimeoutSeconds { get; init; } = 600;

    [YamlMember(Alias = "retry_attempts")]
    public int RetryAttempts { get; init; } = 3;

    [YamlMember(Alias = "retry_delay_seconds")]
    public int RetryDelaySeconds { get; init; } = 30;
}

public class MemoryConfig
{
    [YamlMember(Alias = "store_type")]
    public string StoreType { get; init; } = "git-backed-filesystem";

    [YamlMember(Alias = "base_path")]
    public string BasePath { get; init; } = ".ai/memory";

    [YamlMember(Alias = "auto_commit")]
    public bool AutoCommit { get; init; } = true;
}

public class LoggingConfig
{
    [YamlMember(Alias = "level")]
    public string Level { get; init; } = "info";

    [YamlMember(Alias = "format")]
    public string Format { get; init; } = "json";

    [YamlMember(Alias = "output")]
    public string Output { get; init; } = "console";
}

public class SecurityConfig
{
    [YamlMember(Alias = "secret_scanning")]
    public bool SecretScanning { get; init; } = true;

    [YamlMember(Alias = "require_review_for")]
    public List<string> RequireReviewFor { get; init; } = new();

    [YamlMember(Alias = "audit_log")]
    public bool AuditLog { get; init; } = true;
}

public class ProviderConfig
{
    [YamlMember(Alias = "api_key")]
    public string? ApiKey { get; init; }

    [YamlMember(Alias = "organization")]
    public string? Organization { get; init; }

    [YamlMember(Alias = "endpoint")]
    public string? Endpoint { get; init; }

    [YamlMember(Alias = "default_model")]
    public string? DefaultModel { get; init; }

    [YamlMember(Alias = "context_length")]
    public int? ContextLength { get; init; }
}
