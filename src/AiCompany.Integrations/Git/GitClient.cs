using System.Diagnostics;

namespace AiCompany.Integrations.Git;

public class GitClient
{
    public async Task<bool> IsRepositoryAsync(string? path = null, CancellationToken ct = default)
    {
        var result = await RunAsync("rev-parse --is-inside-work-tree", path, ct);
        return result.ExitCode == 0;
    }

    public async Task<string> GetCurrentBranchAsync(string? path = null, CancellationToken ct = default)
    {
        var result = await RunAsync("rev-parse --abbrev-ref HEAD", path, ct);
        return result.ExitCode == 0 ? result.Stdout.Trim() : "unknown";
    }

    public async Task<string> StatusAsync(string? path = null, CancellationToken ct = default)
    {
        var result = await RunAsync("status --short", path, ct);
        return result.Stdout;
    }

    public async Task<(bool Success, string Stdout, string Stderr)> CommitAsync(
        string message, string? path = null, CancellationToken ct = default)
    {
        var addResult = await RunAsync("add .", path, ct);
        if (addResult.ExitCode != 0)
            return (false, addResult.Stdout, addResult.Stderr);

        var commitResult = await RunAsync(
            $"commit -m \"{message.Replace("\"", "\\\"")}\" --no-gpg-sign", path, ct);
        return (commitResult.ExitCode == 0, commitResult.Stdout, commitResult.Stderr);
    }

    public async Task<(bool Success, string Stdout, string Stderr)> PushAsync(
        string? remote = null, string? branch = null, string? path = null, CancellationToken ct = default)
    {
        remote ??= "origin";
        branch ??= await GetCurrentBranchAsync(path, ct);
        var result = await RunAsync($"push {remote} {branch}", path, ct);
        return (result.ExitCode == 0, result.Stdout, result.Stderr);
    }

    private static async Task<(int ExitCode, string Stdout, string Stderr)> RunAsync(
        string arguments, string? path, CancellationToken ct)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = path ?? Directory.GetCurrentDirectory()
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        var stdout = await process.StandardOutput.ReadToEndAsync();
        var stderr = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync(ct);

        return (process.ExitCode, stdout, stderr);
    }
}
