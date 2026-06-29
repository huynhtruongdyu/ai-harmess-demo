namespace AiCompany.Memory;

public interface IMemoryStore
{
    Task<string> ReadAsync(string storeName, CancellationToken ct = default);
    Task WriteAsync(string storeName, string content, CancellationToken ct = default);
    Task AppendAsync(string storeName, string content, CancellationToken ct = default);
    Task<IReadOnlyList<MemoryEntry>> SearchAsync(string query, int maxResults = 10, CancellationToken ct = default);
    Task<IReadOnlyList<MemoryEntry>> GetHistoryAsync(string storeName, int count = 10, CancellationToken ct = default);
    Task<bool> ExistsAsync(string storeName, CancellationToken ct = default);
}

public record MemoryEntry(
    string StoreName,
    string Content,
    string? Author,
    DateTime Timestamp,
    int Version,
    string? CommitHash = null
);
