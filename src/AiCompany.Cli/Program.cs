using System.CommandLine;
using AiCompany.Cli.Commands;

namespace AiCompany.Cli;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCmd = new RootCommand("Harness AI Software Company — multi-agent development platform");

        rootCmd.AddCommand(new InitCommand());
        rootCmd.AddCommand(new RunCommand());
        rootCmd.AddCommand(new AgentCommand());
        rootCmd.AddCommand(new ValidateCommand());
        rootCmd.AddCommand(new ListCommand());
        rootCmd.AddCommand(new StatusCommand());

        return await rootCmd.InvokeAsync(args);
    }
}
