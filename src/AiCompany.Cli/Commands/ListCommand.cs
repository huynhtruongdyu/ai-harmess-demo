using System.CommandLine;
using AiCompany.Configuration;

namespace AiCompany.Cli.Commands;

public class ListCommand : Command
{
    public ListCommand() : base("list", "List available agents or workflows")
    {
        var typeArg = new Argument<string>("type", "List type: 'agents' or 'workflows'");

        AddArgument(typeArg);

        this.SetHandler((string type) =>
        {
            var configLoader = new ConfigLoader();

            switch (type.ToLowerInvariant())
            {
                case "agents":
                    Console.WriteLine("Available agents:");
                    foreach (var agent in configLoader.ListAgents())
                    {
                        Console.WriteLine($"  - {agent}");
                        var readmePath = Path.Combine(".ai", "agents", agent, "README.md");
                        if (File.Exists(readmePath))
                        {
                            var firstLine = File.ReadLines(readmePath).FirstOrDefault(l =>
                                l.StartsWith("#") && !l.StartsWith("##"));
                            if (firstLine != null)
                                Console.WriteLine($"    {firstLine.Replace("#", "").Trim()}");
                        }
                    }
                    break;

                case "workflows":
                    Console.WriteLine("Available workflows:");
                    foreach (var wf in configLoader.ListWorkflows())
                    {
                        try
                        {
                            var def = configLoader.LoadWorkflow(wf);
                            Console.WriteLine($"  - {wf}: {def.Description}");
                        }
                        catch
                        {
                            Console.WriteLine($"  - {wf}");
                        }
                    }
                    break;

                default:
                    Console.Error.WriteLine("Usage: ai-company list <agents|workflows>");
                    Environment.ExitCode = 1;
                    break;
            }
        }, typeArg);
    }
}
