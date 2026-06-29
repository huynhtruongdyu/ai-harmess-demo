using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AiCompany.Configuration;

public partial class ConfigLoader
{
    private static readonly Regex EnvVarPattern = EnvVarRegex();

    [GeneratedRegex(@"\$\{([^}]+)\}", RegexOptions.Compiled)]
    private static partial Regex EnvVarRegex();

    private readonly IDeserializer _deserializer;
    private readonly string _basePath;
    private readonly bool _resolveEnvVars;

    public ConfigLoader(string? basePath = null, bool resolveEnvVars = true)
    {
        _basePath = basePath ?? Directory.GetCurrentDirectory();
        _resolveEnvVars = resolveEnvVars;
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
    }

    public static string ResolveEnvironmentVariables(string input)
    {
        return EnvVarPattern.Replace(input, match =>
        {
            var expr = match.Groups[1].Value;
            string? value;
            string? defaultValue = null;

            if (expr.Contains(":-"))
            {
                var parts = expr.Split([":-"], 2, StringSplitOptions.None);
                value = Environment.GetEnvironmentVariable(parts[0]);
                defaultValue = parts[1];
            }
            else if (expr.Contains(":?"))
            {
                var parts = expr.Split([":?"], 2, StringSplitOptions.None);
                value = Environment.GetEnvironmentVariable(parts[0]);
                if (string.IsNullOrEmpty(value))
                    throw new ConfigException(
                        $"Required environment variable '{parts[0]}' is not set: {parts[1]}");
                return value;
            }
            else
            {
                value = Environment.GetEnvironmentVariable(expr);
            }

            if (string.IsNullOrEmpty(value))
            {
                if (defaultValue != null)
                    return defaultValue;
                throw new ConfigException(
                    $"Environment variable '{expr}' is not set. " +
                    $"Set it or use '${{{expr}:<default>}}' syntax.");
            }

            return value;
        });
    }

    public PlatformConfig LoadPlatformConfig()
    {
        var path = Path.Combine(_basePath, ".ai", "config.yaml");
        return LoadFile<PlatformConfig>(path);
    }

    public RoutingConfig LoadRoutingConfig()
    {
        var path = Path.Combine(_basePath, ".ai", "routing.yaml");
        return LoadFile<RoutingConfig>(path);
    }

    public AgentConfig LoadAgentConfig(string agentName)
    {
        var path = Path.Combine(_basePath, ".ai", "agents", agentName, "config.yaml");
        return LoadFile<AgentConfig>(path);
    }

    public WorkflowDefinition LoadWorkflow(string workflowName)
    {
        var path = Path.Combine(_basePath, ".ai", "workflows", $"{workflowName}.yaml");
        return LoadFile<WorkflowDefinition>(path);
    }

    public List<string> ListWorkflows()
    {
        var workflowsDir = Path.Combine(_basePath, ".ai", "workflows");
        if (!Directory.Exists(workflowsDir))
            return new List<string>();

        return Directory.GetFiles(workflowsDir, "*.yaml")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(x => x != null)
            .Cast<string>()
            .ToList();
    }

    public List<string> ListAgents()
    {
        var agentsDir = Path.Combine(_basePath, ".ai", "agents");
        if (!Directory.Exists(agentsDir))
            return new List<string>();

        return Directory.GetDirectories(agentsDir)
            .Select(Path.GetFileName)
            .Where(x => x != null)
            .Cast<string>()
            .ToList();
    }

    private T LoadFile<T>(string path) where T : new()
    {
        if (!File.Exists(path))
            throw new ConfigException($"Configuration file not found: {path}");

        var yaml = File.ReadAllText(path);
        if (_resolveEnvVars)
            yaml = ResolveEnvironmentVariables(yaml);

        try
        {
            var result = _deserializer.Deserialize<T>(yaml);
            if (result == null)
                throw new ConfigException($"Failed to parse config file: {path}");
            return result;
        }
        catch (YamlDotNet.Core.YamlException ex)
        {
            throw new ConfigException($"YAML parse error in {path}: {ex.Message}", ex);
        }
    }
}
