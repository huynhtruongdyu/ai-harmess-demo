using System.Collections.Concurrent;

namespace AiCompany.Memory;

public class FileSystemMemoryStore : IMemoryStore
{
    private readonly string _basePath;
    private readonly ConcurrentDictionary<string, object> _locks = new();

    public FileSystemMemoryStore(string? basePath = null)
    {
        _basePath = basePath ?? Path.Combine(Directory.GetCurrentDirectory(), ".ai", "memory");
        Directory.CreateDirectory(_basePath);
    }

    public Task<string> ReadAsync(string storeName, CancellationToken ct = default)
    {
        var path = GetPath(storeName);
        if (!File.Exists(path))
            return Task.FromResult(string.Empty);

        return File.ReadAllTextAsync(path, ct);
    }

    public async Task WriteAsync(string storeName, string content, CancellationToken ct = default)
    {
        var path = GetPath(storeName);
        var lockObj = _locks.GetOrAdd(storeName, _ => new());

        lock (lockObj)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, content);
        }

        await Task.CompletedTask;
    }

    public async Task AppendAsync(string storeName, string content, CancellationToken ct = default)
    {
        var path = GetPath(storeName);
        var lockObj = _locks.GetOrAdd(storeName, _ => new());

        lock (lockObj)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.AppendAllText(path, $"\n{content}");
        }

        await Task.CompletedTask;
    }

    public Task<IReadOnlyList<MemoryEntry>> SearchAsync(string query, int maxResults = 10, CancellationToken ct = default)
    {
        var results = new List<MemoryEntry>();
        var dir = new DirectoryInfo(_basePath);

        foreach (var file in dir.GetFiles("*.md").Take(maxResults))
        {
            var content = File.ReadAllText(file.FullName);
            if (content.Contains(query, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(new MemoryEntry(
                    Path.GetFileNameWithoutExtension(file.Name),
                    content.Length > 200 ? content[..200] : content,
                    null,
                    file.LastWriteTimeUtc,
                    1
                ));
            }
        }

        return Task.FromResult<IReadOnlyList<MemoryEntry>>(results);
    }

    public Task<IReadOnlyList<MemoryEntry>> GetHistoryAsync(string storeName, int count = 10, CancellationToken ct = default)
    {
        var path = GetPath(storeName);
        if (!File.Exists(path))
            return Task.FromResult<IReadOnlyList<MemoryEntry>>(new List<MemoryEntry>());

        var content = File.ReadAllText(path);
        var entry = new MemoryEntry(storeName, content, null, File.GetLastWriteTimeUtc(path), 1);
        return Task.FromResult<IReadOnlyList<MemoryEntry>>(new[] { entry });
    }

    public Task<bool> ExistsAsync(string storeName, CancellationToken ct = default)
    {
        return Task.FromResult(File.Exists(GetPath(storeName)));
    }

    private string GetPath(string storeName)
    {
        var safeName = storeName
            .Replace("/", Path.DirectorySeparatorChar.ToString())
            .Replace("\\", Path.DirectorySeparatorChar.ToString());
        return Path.Combine(_basePath, $"{safeName}.md");
    }
}
