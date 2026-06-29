using System.CommandLine;

namespace AiCompany.Cli.Commands;

public class InitCommand : Command
{
    public InitCommand() : base("init", "Initialize the AI company project structure")
    {
        var forceOption = new Option<bool>("--force", "Overwrite existing files");
        AddOption(forceOption);

        this.SetHandler(async (bool force) =>
        {
            Console.WriteLine("Initializing AI Software Company...");

            var dirs = new[]
            {
                ".ai/agents", ".ai/workflows", ".ai/prompts",
                ".ai/memory", ".ai/templates", ".ai/schemas", ".ai/tools",
                "docs", "src", "tests"
            };

            foreach (var dir in dirs)
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    Console.WriteLine($"  Created: {dir}/");
                }
            }

            await Task.CompletedTask;
        }, forceOption);
    }
}
