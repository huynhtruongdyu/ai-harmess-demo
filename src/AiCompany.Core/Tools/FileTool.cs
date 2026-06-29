using System.Diagnostics;

namespace AiCompany.Tools;

public class FileTool : ITool
{
    public string Name => "file-operations";

    public async Task<ToolResult> ExecuteAsync(ToolInput input, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var op = input.Operation;
            var path = GetParameter<string>(input.Parameters, "path");
            var resolvedPath = ResolvePath(path);

            switch (op)
            {
                case "read":
                    return Ok(sw, await File.ReadAllTextAsync(resolvedPath, ct));

                case "write":
                    var content = GetParameter<string>(input.Parameters, "content");
                    Directory.CreateDirectory(Path.GetDirectoryName(resolvedPath)!);
                    await File.WriteAllTextAsync(resolvedPath, content, ct);
                    return Ok(sw, $"Written {content.Length} bytes to {path}");

                case "list":
                    if (!Directory.Exists(resolvedPath))
                        return Ok(sw, Array.Empty<string>());
                    var files = Directory.GetFileSystemEntries(resolvedPath)
                        .Select(f => Path.GetRelativePath(
                            Path.GetDirectoryName(resolvedPath) ?? ".", f))
                        .ToArray();
                    return Ok(sw, files);

                case "exists":
                    return Ok(sw, File.Exists(resolvedPath) || Directory.Exists(resolvedPath));

                case "delete":
                    if (File.Exists(resolvedPath))
                        File.Delete(resolvedPath);
                    else if (Directory.Exists(resolvedPath))
                        Directory.Delete(resolvedPath, true);
                    return Ok(sw, $"Deleted {path}");

                default:
                    return Error(sw, $"Unsupported file operation: {op}");
            }
        }
        catch (Exception ex)
        {
            return Error(sw, ex.Message);
        }
    }

    private static ToolResult Ok(Stopwatch sw, object data) =>
        new(true, data, null, sw.Elapsed);

    private static ToolResult Error(Stopwatch sw, string message) =>
        new(false, null, message, sw.Elapsed);

    private static T GetParameter<T>(Dictionary<string, object?> parameters, string key)
    {
        if (parameters.TryGetValue(key, out var value) && value is T typed)
            return typed;
        throw new ArgumentException($"Missing or invalid parameter: {key}");
    }

    private static string ResolvePath(string path)
    {
        var root = Directory.GetCurrentDirectory();
        var combined = Path.GetFullPath(Path.Combine(root, path));
        if (!combined.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("Path traversal is not allowed.");
        return combined;
    }
}
