using System.Collections.Concurrent;

namespace AiCompany.Memory;

public class MemoryIndex
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _invertedIndex = new();
    private readonly IMemoryStore _store;
    private bool _built;

    public MemoryIndex(IMemoryStore store)
    {
        _store = store;
    }

    public async Task BuildAsync(CancellationToken ct = default)
    {
        _invertedIndex.Clear();
        var stores = new[] {
            "project-context", "architecture-decisions", "business-rules",
            "coding-standards", "glossary", "current-sprint", "open-issues", "completed-tasks"
        };

        foreach (var store in stores)
        {
            if (ct.IsCancellationRequested) break;
            var content = await _store.ReadAsync(store, ct);
            if (string.IsNullOrEmpty(content)) continue;

            var words = content.Split(new[] { ' ', '\n', '\r', '\t', '.', ',', ';', ':', '!', '?', '(', ')', '[', ']' },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                var lower = word.Trim().ToLowerInvariant();
                if (lower.Length < 3) continue;

                _invertedIndex.AddOrUpdate(lower,
                    _ => new HashSet<string> { store },
                    (_, set) => { set.Add(store); return set; });
            }
        }

        _built = true;
    }

    public string[] Search(string query, int maxResults = 5)
    {
        if (!_built || string.IsNullOrWhiteSpace(query))
            return Array.Empty<string>();

        var terms = query.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim().ToLowerInvariant())
            .Where(t => t.Length >= 3)
            .ToArray();

        if (terms.Length == 0)
            return Array.Empty<string>();

        var scores = new Dictionary<string, int>();
        foreach (var term in terms)
        {
            foreach (var (key, stores) in _invertedIndex)
            {
                if (!key.Contains(term, StringComparison.OrdinalIgnoreCase))
                    continue;

                foreach (var store in stores)
                {
                    scores.TryGetValue(store, out var current);
                    scores[store] = current + 1;
                }
            }
        }

        return scores.OrderByDescending(kv => kv.Value)
            .Take(maxResults)
            .Select(kv => kv.Key)
            .ToArray();
    }
}
