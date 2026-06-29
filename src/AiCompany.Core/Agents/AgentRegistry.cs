using System.Collections.Concurrent;

namespace AiCompany.Agents;

public class AgentRegistry
{
    private readonly ConcurrentDictionary<string, IAgent> _agents = new();

    public void Register(IAgent agent)
    {
        if (!_agents.TryAdd(agent.Name, agent))
            throw new InvalidOperationException($"Agent '{agent.Name}' is already registered.");
    }

    public IAgent Get(string name)
    {
        if (_agents.TryGetValue(name, out var agent))
            return agent;
        throw new KeyNotFoundException($"Agent '{name}' is not registered.");
    }

    public bool IsRegistered(string name) => _agents.ContainsKey(name);

    public IReadOnlyCollection<string> ListAgents() =>
        _agents.Keys.OrderBy(x => x).ToList().AsReadOnly();

    public string? FindAgentByTaskType(string taskType, string[] keywords)
    {
        foreach (var (name, _) in _agents.OrderBy(x => x.Key))
        {
            if (keywords.Any(k => name.Contains(k, StringComparison.OrdinalIgnoreCase)))
                return name;
        }
        return null;
    }
}
