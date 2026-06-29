using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace AiCompany.Workflows;

public class RuntimeStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _runtimeDir;
    private readonly ILogger<RuntimeStore> _logger;

    public RuntimeStore(string basePath, ILogger<RuntimeStore> logger)
    {
        _runtimeDir = Path.Combine(basePath, ".ai", "_runtime");
        _logger = logger;
        Directory.CreateDirectory(_runtimeDir);
    }

    public string SaveRun(WorkflowRun run)
    {
        var filePath = Path.Combine(_runtimeDir, $"{run.Id}.json");
        var json = JsonSerializer.Serialize(run, JsonOptions);
        File.WriteAllText(filePath, json);
        _logger.LogDebug("Saved run {RunId} to {Path}", run.Id, filePath);
        return filePath;
    }

    public WorkflowRun? GetRun(string runId)
    {
        var filePath = Path.Combine(_runtimeDir, $"{runId}.json");
        if (!File.Exists(filePath)) return null;
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<WorkflowRun>(json, JsonOptions);
    }

    public IReadOnlyList<WorkflowRun> ListRuns()
    {
        if (!Directory.Exists(_runtimeDir)) return Array.Empty<WorkflowRun>();
        return Directory.GetFiles(_runtimeDir, "*.json")
            .Select(f => JsonSerializer.Deserialize<WorkflowRun>(File.ReadAllText(f), JsonOptions))
            .Where(r => r != null)
            .Select(r => r!)
            .OrderByDescending(r => r.StartedAt)
            .ToList()
            .AsReadOnly();
    }

    public IEnumerable<WorkflowRun> GetActiveRuns()
    {
        return ListRuns().Where(r => r.Status == WorkflowStatus.Running);
    }

    public void DeleteRun(string runId)
    {
        var filePath = Path.Combine(_runtimeDir, $"{runId}.json");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            _logger.LogDebug("Deleted run {RunId}", runId);
        }
    }
}
