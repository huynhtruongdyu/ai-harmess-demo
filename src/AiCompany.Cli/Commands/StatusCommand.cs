using System.CommandLine;

namespace AiCompany.Cli.Commands;

public class StatusCommand : Command
{
    public StatusCommand() : base("status", "Show current project status")
    {
        this.SetHandler(() =>
        {
            var root = Directory.GetCurrentDirectory();

            Console.WriteLine("AiCompany Status\n");

            // Check .ai directory structure
            Console.WriteLine("Structure:");
            CheckPath(".ai/config.yaml", "Platform config");
            CheckPath(".ai/routing.yaml", "Routing config");
            CheckPath(".ai/agents/", "Agents directory");
            CheckPath(".ai/workflows/", "Workflows directory");
            CheckPath(".ai/memory/", "Memory directory");

            Console.WriteLine();

            // Count agents
            var agentsDir = Path.Combine(root, ".ai", "agents");
            if (Directory.Exists(agentsDir))
            {
                var agents = Directory.GetDirectories(agentsDir).Select(Path.GetFileName).ToArray();
                Console.WriteLine($"Agents: {agents.Length} registered");
                foreach (var a in agents)
                    Console.WriteLine($"  - {a}");
            }

            Console.WriteLine();

            // Count workflows
            var workflowsDir = Path.Combine(root, ".ai", "workflows");
            if (Directory.Exists(workflowsDir))
            {
                var workflows = Directory.GetFiles(workflowsDir, "*.yaml")
                    .Select(Path.GetFileNameWithoutExtension).ToArray();
                Console.WriteLine($"Workflows: {workflows.Length} defined");
                foreach (var w in workflows)
                    Console.WriteLine($"  - {w}");
            }

            Console.WriteLine();

            // Check memory stores
            Console.WriteLine("Memory stores:");
            var memoryDir = Path.Combine(root, ".ai", "memory");
            if (Directory.Exists(memoryDir))
            {
                foreach (var f in Directory.GetFiles(memoryDir, "*.md"))
                {
                    var info = new FileInfo(f);
                    Console.WriteLine($"  - {Path.GetFileNameWithoutExtension(f)} ({info.Length} bytes)");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Run 'ai-company validate' for detailed validation.");
        });
    }

    private static void CheckPath(string relativePath, string label)
    {
        var full = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        var exists = relativePath.EndsWith("/")
            ? Directory.Exists(full)
            : File.Exists(full);
        var icon = exists ? "✓" : "✗";
        Console.WriteLine($"  {icon} {label} ({relativePath})");
    }
}
