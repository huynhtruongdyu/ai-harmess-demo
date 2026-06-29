using System.CommandLine;
using AiCompany.Agents;
using AiCompany.Configuration;
using AiCompany.Memory;
using AiCompany.Tools;
using AiCompany.Workflows;
using Microsoft.Extensions.Logging;

namespace AiCompany.Cli.Commands;

public class RunCommand : Command
{
    public RunCommand() : base("run", "Execute a workflow")
    {
        var workflowArg = new Argument<string>("workflow", "Workflow name to execute");
        var inputOption = new Option<string[]>("--input", "Input parameters (key=value)")
        { AllowMultipleArgumentsPerToken = true };
        var dryRunOption = new Option<bool>("--dry-run", "Validate without executing");

        AddArgument(workflowArg);
        AddOption(inputOption);
        AddOption(dryRunOption);

        this.SetHandler(async (string workflow, string[] inputs, bool dryRun) =>
        {
            try
            {
                var configLoader = new ConfigLoader();
                var workflowDef = configLoader.LoadWorkflow(workflow);

                Console.WriteLine($"Workflow: {workflowDef.Name}");
                Console.WriteLine($"Description: {workflowDef.Description}");
                Console.WriteLine($"Steps: {workflowDef.Steps.Count}");
                Console.WriteLine();

                if (dryRun)
                {
                    foreach (var step in workflowDef.Steps)
                    {
                        Console.WriteLine($"  [{step.Id}] {step.Agent}.{step.Action}");
                        if (step.Gate?.RequiresApproval == true)
                            Console.WriteLine($"    ⚠ Gate: approval required");
                    }
                    Console.WriteLine("\nDry-run: validation passed.");
                    return;
                }

                var inputDict = inputs?
                    .Select(i => i.Split('=', 2))
                    .Where(p => p.Length == 2)
                    .ToDictionary(p => p[0], p => p[1])
                    ?? new Dictionary<string, string>();

                using var loggerFactory = LoggerFactory.Create(b =>
                    b.AddConsole().SetMinimumLevel(LogLevel.Information));

                var memory = new FileSystemMemoryStore();
                var toolRegistry = new ToolRegistry();
                toolRegistry.Register(new FileTool());
                toolRegistry.Register(new ShellTool());

                var agentRegistry = new AgentRegistry();

                foreach (var agentName in configLoader.ListAgents())
                {
                    try
                    {
                        var agentConfig = configLoader.LoadAgentConfig(agentName);
                        var runtime = new AgentRuntime(agentConfig, memory, toolRegistry,
                            loggerFactory.CreateLogger<AgentRuntime>());
                        LLMHelper.TryWireLlmProcessor(runtime, configLoader);
                        agentRegistry.Register(runtime);
                        Console.WriteLine($"  Loaded agent: {agentName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  Skipped agent '{agentName}': {ex.Message}");
                    }
                }

                var systemAgent = new SystemAgent(memory, toolRegistry, configLoader,
                    loggerFactory.CreateLogger<SystemAgent>());
                agentRegistry.Register(systemAgent);

                var runtimeStore = new RuntimeStore(
                    Directory.GetCurrentDirectory(),
                    loggerFactory.CreateLogger<RuntimeStore>());
                var engine = new WorkflowEngine(agentRegistry, configLoader, runtimeStore,
                    loggerFactory.CreateLogger<WorkflowEngine>());

                Console.WriteLine($"\nExecuting workflow: {workflow}...\n");
                var result = await engine.ExecuteAsync(workflow, inputDict);

                Console.WriteLine($"Status: {result.Status}");
                Console.WriteLine($"Steps: {result.Steps.Count}");

                foreach (var step in result.Steps)
                {
                    var icon = step.Success ? "✓" : "✗";
                    Console.WriteLine($"  {icon} [{step.StepId}] {step.Agent} — {step.Status} ({step.Duration.TotalSeconds:F1}s)");
                }

                if (result.Error != null)
                    Console.WriteLine($"\nError: {result.Error}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Environment.ExitCode = 1;
            }
        }, workflowArg, inputOption, dryRunOption);
    }
}
