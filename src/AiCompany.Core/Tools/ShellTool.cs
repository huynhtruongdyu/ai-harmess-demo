using System.Collections.Concurrent;
using System.Diagnostics;

namespace AiCompany.Tools;

public class ShellTool : ITool
{
    private static readonly HashSet<string> AllowlistedCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "dotnet", "npm", "node", "git", "make", "pwsh", "powershell", "bash", "sh"
    };

    private static readonly HashSet<string> BlockedPatterns = new(StringComparer.OrdinalIgnoreCase)
    {
        "rm -rf /", "rm -rf --no-preserve-root", "format C:", "> /dev/sda",
        "dd if=", "mkfs.", ":(){ :|:& };:", "chmod 777 /"
    };

    public string Name => "run-commands";

    public async Task<ToolResult> ExecuteAsync(ToolInput input, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var command = GetParameter<string>(input.Parameters, "command");
            var workingDir = GetParameter<string?>(input.Parameters, "working_directory") ?? ".";
            var timeout = GetParameter<int?>(input.Parameters, "timeout_seconds") ?? 120;

            ValidateCommand(command);

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project src/AiCompany.Cli -- {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = ResolvePath(workingDir)
            };

            using var process = new Process { StartInfo = psi };
            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync(ct);

            return new ToolResult(
                process.ExitCode == 0,
                new { stdout = output, exitCode = process.ExitCode },
                string.IsNullOrEmpty(error) ? null : error,
                sw.Elapsed
            );
        }
        catch (Exception ex)
        {
            return new ToolResult(false, null, ex.Message, sw.Elapsed);
        }
    }

    private static void ValidateCommand(string command)
    {
        foreach (var pattern in BlockedPatterns)
        {
            if (command.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException($"Command blocked (matches unsafe pattern).");
        }
    }

    private static T GetParameter<T>(Dictionary<string, object?> parameters, string key)
    {
        if (parameters.TryGetValue(key, out var value) && value is T typed)
            return typed;
        return default!;
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
