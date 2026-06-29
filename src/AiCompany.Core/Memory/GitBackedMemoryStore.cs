namespace AiCompany.Memory;

public class GitBackedMemoryStore : IMemoryStore
{
    private readonly IMemoryStore _inner;

    public GitBackedMemoryStore(IMemoryStore inner)
    {
        _inner = inner;
    }

    public async Task<string> ReadAsync(string storeName, CancellationToken ct = default)
    {
        return await _inner.ReadAsync(storeName, ct);
    }

    public async Task WriteAsync(string storeName, string content, CancellationToken ct = default)
    {
        await _inner.WriteAsync(storeName, content, ct);
        await TryCommitAsync($"Update {storeName}", ct);
    }

    public async Task AppendAsync(string storeName, string content, CancellationToken ct = default)
    {
        await _inner.AppendAsync(storeName, content, ct);
        await TryCommitAsync($"Append to {storeName}", ct);
    }

    public async Task<IReadOnlyList<MemoryEntry>> SearchAsync(string query, int maxResults = 10, CancellationToken ct = default)
    {
        return await _inner.SearchAsync(query, maxResults, ct);
    }

    public async Task<IReadOnlyList<MemoryEntry>> GetHistoryAsync(string storeName, int count = 10, CancellationToken ct = default)
    {
        return await _inner.GetHistoryAsync(storeName, count, ct);
    }

    public async Task<bool> ExistsAsync(string storeName, CancellationToken ct = default)
    {
        return await _inner.ExistsAsync(storeName, ct);
    }

    private static async Task TryCommitAsync(string message, CancellationToken ct)
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo("git")
            {
                Arguments = $"add .ai/memory/ && git commit -m \"memory: {message}\" --no-gpg-sign",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = System.Diagnostics.Process.Start(psi);
            if (process != null)
                await process.WaitForExitAsync(ct);
        }
        catch
        {
            // Git not available or not a repo — write silently
        }
    }
}
