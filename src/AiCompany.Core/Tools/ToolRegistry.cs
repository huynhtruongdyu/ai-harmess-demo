using System.Collections.Concurrent;

namespace AiCompany.Tools;

public class ToolRegistry
{
    private readonly ConcurrentDictionary<string, ITool> _tools = new();

    public void Register(ITool tool)
    {
        if (!_tools.TryAdd(tool.Name, tool))
            throw new InvalidOperationException($"Tool '{tool.Name}' is already registered.");
    }

    public ITool Get(string name)
    {
        if (_tools.TryGetValue(name, out var tool))
            return tool;
        throw new KeyNotFoundException($"Tool '{name}' is not registered.");
    }

    public bool IsRegistered(string name) => _tools.ContainsKey(name);

    public IReadOnlyCollection<string> ListTools() => _tools.Keys.ToList().AsReadOnly();

    public void RegisterRange(IEnumerable<ITool> tools)
    {
        foreach (var tool in tools)
            Register(tool);
    }
}
