namespace AiCompany.Validation;

public class SchemaValidator
{
    private readonly string _schemaPath;

    public SchemaValidator(string? basePath = null)
    {
        _schemaPath = Path.Combine(basePath ?? Directory.GetCurrentDirectory(), ".ai", "schemas");
    }

    public ValidationResult ValidateAgentConfig(string agentName)
    {
        var configPath = Path.Combine(
            Directory.GetCurrentDirectory(), ".ai", "agents", agentName, "config.yaml");

        var errors = new List<string>();
        var warnings = new List<string>();

        if (!File.Exists(configPath))
        {
            errors.Add($"Agent config not found: {configPath}");
            return new ValidationResult(false, errors, warnings);
        }

        try
        {
            var yaml = File.ReadAllText(configPath);

            if (!yaml.Contains("agent:"))
                errors.Add("Missing top-level 'agent' key");

            if (!yaml.Contains("name:"))
                errors.Add("Missing required field: name");

            if (!yaml.Contains("model:"))
                warnings.Add("Missing recommended field: model (will use default)");

            if (!yaml.Contains("tools:"))
                warnings.Add("Missing recommended field: tools (agent will have no tools)");
        }
        catch (Exception ex)
        {
            errors.Add($"Parse error: {ex.Message}");
        }

        return new ValidationResult(errors.Count == 0, errors, warnings);
    }

    public ValidationResult ValidateWorkflow(string workflowName)
    {
        var path = Path.Combine(
            Directory.GetCurrentDirectory(), ".ai", "workflows", $"{workflowName}.yaml");

        var errors = new List<string>();
        var warnings = new List<string>();

        if (!File.Exists(path))
        {
            errors.Add($"Workflow not found: {path}");
            return new ValidationResult(false, errors, warnings);
        }

        try
        {
            var yaml = File.ReadAllText(path);

            if (!yaml.Contains("name:"))
                errors.Add("Missing required field: name");

            if (!yaml.Contains("steps:"))
                errors.Add("Missing required field: steps");

            if (!yaml.Contains("trigger:"))
                errors.Add("Missing required field: trigger");

            if (!yaml.Contains("event:"))
                errors.Add("Trigger missing required field: event");
        }
        catch (Exception ex)
        {
            errors.Add($"Parse error: {ex.Message}");
        }

        return new ValidationResult(errors.Count == 0, errors, warnings);
    }

    public ValidationResult ValidateAll()
    {
        var errors = new List<string>();
        var warnings = new List<string>();

        // Validate all agents
        var agentsDir = Path.Combine(Directory.GetCurrentDirectory(), ".ai", "agents");
        if (Directory.Exists(agentsDir))
        {
            foreach (var agentDir in Directory.GetDirectories(agentsDir))
            {
                var name = Path.GetFileName(agentDir);
                var result = ValidateAgentConfig(name);
                errors.AddRange(result.Errors.Select(e => $"[{name}] {e}"));
                warnings.AddRange(result.Warnings.Select(w => $"[{name}] {w}"));
            }
        }

        // Validate all workflows
        var workflowsDir = Path.Combine(Directory.GetCurrentDirectory(), ".ai", "workflows");
        if (Directory.Exists(workflowsDir))
        {
            foreach (var wf in Directory.GetFiles(workflowsDir, "*.yaml"))
            {
                var name = Path.GetFileNameWithoutExtension(wf);
                var result = ValidateWorkflow(name);
                errors.AddRange(result.Errors.Select(e => $"[{name}] {e}"));
                warnings.AddRange(result.Warnings.Select(w => $"[{name}] {w}"));
            }
        }

        return new ValidationResult(errors.Count == 0, errors, warnings);
    }
}

public record ValidationResult(bool IsValid, List<string> Errors, List<string> Warnings);
