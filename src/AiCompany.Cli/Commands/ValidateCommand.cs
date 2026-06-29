using System.CommandLine;
using AiCompany.Validation;

namespace AiCompany.Cli.Commands;

public class ValidateCommand : Command
{
    public ValidateCommand() : base("validate", "Validate AI platform configurations")
    {
        this.SetHandler(() =>
        {
            Console.WriteLine("Validating AI Software Company configuration...\n");

            var validator = new SchemaValidator();

            // Validate core config files
            ValidateFile("config.yaml", ".ai/config.yaml");
            ValidateFile("routing.yaml", ".ai/routing.yaml");

            // Validate all agents
            Console.WriteLine("Checking agents...");
            var agentsDir = Path.Combine(Directory.GetCurrentDirectory(), ".ai", "agents");
            if (Directory.Exists(agentsDir))
            {
                var agentCount = 0;
                foreach (var agentDir in Directory.GetDirectories(agentsDir))
                {
                    var name = Path.GetFileName(agentDir);
                    var result = validator.ValidateAgentConfig(name);

                    if (result.IsValid)
                    {
                        Console.WriteLine($"  ✓ {name}");
                    }
                    else
                    {
                        Console.WriteLine($"  ✗ {name}");
                        foreach (var e in result.Errors)
                            Console.WriteLine($"      {e}");
                    }
                    agentCount++;
                }
                Console.WriteLine($"  ({agentCount} agents checked)\n");
            }

            // Validate all workflows
            Console.WriteLine("Checking workflows...");
            var workflowsDir = Path.Combine(Directory.GetCurrentDirectory(), ".ai", "workflows");
            if (Directory.Exists(workflowsDir))
            {
                var wfCount = 0;
                foreach (var wf in Directory.GetFiles(workflowsDir, "*.yaml"))
                {
                    var name = Path.GetFileNameWithoutExtension(wf);
                    var result = validator.ValidateWorkflow(name);

                    if (result.IsValid)
                    {
                        Console.WriteLine($"  ✓ {name}");
                    }
                    else
                    {
                        Console.WriteLine($"  ✗ {name}");
                        foreach (var e in result.Errors)
                            Console.WriteLine($"      {e}");
                    }
                    wfCount++;
                }
                Console.WriteLine($"  ({wfCount} workflows checked)\n");
            }

            // Full validation
            Console.WriteLine("Running full validation...");
            var fullResult = validator.ValidateAll();

            Console.WriteLine();
            if (fullResult.IsValid)
                Console.WriteLine("✓ All configurations valid.");
            else
            {
                Console.WriteLine($"✗ {fullResult.Errors.Count} errors found:");
                foreach (var e in fullResult.Errors)
                    Console.WriteLine($"  - {e}");
            }

            if (fullResult.Warnings.Count > 0)
            {
                Console.WriteLine($"\n⚠ {fullResult.Warnings.Count} warnings:");
                foreach (var w in fullResult.Warnings)
                    Console.WriteLine($"  - {w}");
            }
        });
    }

    private static void ValidateFile(string label, string path)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
        if (File.Exists(fullPath))
            Console.WriteLine($"  ✓ {label}");
        else
            Console.WriteLine($"  ✗ {label} (not found at {path})");
    }
}
