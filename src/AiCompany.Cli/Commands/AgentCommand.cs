using System.CommandLine;
using AiCompany.Agents;
using AiCompany.Configuration;
using AiCompany.Memory;
using AiCompany.Tools;
using Microsoft.Extensions.Logging;

namespace AiCompany.Cli.Commands;

public class AgentCommand : Command
{
    public AgentCommand() : base("agent", "Interact directly with an agent")
    {
        var nameArg = new Argument<string>("name", "Agent name");
        var promptArg = new Argument<string>("prompt", "Prompt or task description");
        var contextOption = new Option<string[]>("--context", "Context files to include")
        { AllowMultipleArgumentsPerToken = true };

        AddArgument(nameArg);
        AddArgument(promptArg);
        AddOption(contextOption);

        this.SetHandler(async (string name, string prompt, string[] contextFiles) =>
        {
            try
            {
                var configLoader = new ConfigLoader();
                var agentConfig = configLoader.LoadAgentConfig(name);

                using var loggerFactory = LoggerFactory.Create(b =>
                    b.AddConsole().SetMinimumLevel(LogLevel.Warning));

                var memory = new FileSystemMemoryStore();
                var toolRegistry = new ToolRegistry();
                toolRegistry.Register(new FileTool());

                var agent = new AgentRuntime(agentConfig, memory, toolRegistry,
                    loggerFactory.CreateLogger<AgentRuntime>());
                LLMHelper.TryWireLlmProcessor(agent, configLoader);

                Console.WriteLine($"Agent: {name}");
                Console.WriteLine($"Prompt: {prompt[..Math.Min(prompt.Length, 80)]}...\n");

                var task = new AgentTask(
                    Id: $"cli-{DateTime.UtcNow:yyyyMMdd-HHmmss}",
                    AgentName: name,
                    Action: "chat",
                    Input: prompt,
                    ContextFiles: contextFiles
                );

                var result = await agent.ExecuteAsync(task, CancellationToken.None);

                if (result.Success)
                {
                    Console.WriteLine(result.Output);
                }
                else
                {
                    Console.Error.WriteLine($"Agent failed: {result.Error}");
                    Environment.ExitCode = 1;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Environment.ExitCode = 1;
            }
        }, nameArg, promptArg, contextOption);
    }
}
