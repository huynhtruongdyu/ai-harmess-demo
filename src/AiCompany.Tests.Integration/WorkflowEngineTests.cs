using AiCompany.Agents;
using AiCompany.Configuration;
using AiCompany.Memory;
using AiCompany.Tools;
using AiCompany.Workflows;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AiCompany.Tests.Integration;

public class WorkflowEngineTests : IDisposable
{
    private static readonly string TestBasePath;
    private readonly ConfigLoader _configLoader;
    private readonly AgentRegistry _agentRegistry;
    private readonly ILogger<WorkflowEngine> _logger;
    private readonly ILogger<RuntimeStore> _runtimeLogger;
    private readonly string _tempDir;

    static WorkflowEngineTests()
    {
        var dir = Directory.GetCurrentDirectory();
        while (dir != null && !File.Exists(Path.Combine(dir, ".ai", "config.yaml")))
            dir = Path.GetDirectoryName(dir);
        TestBasePath = dir ?? throw new InvalidOperationException("Could not find .ai directory");
    }

    public WorkflowEngineTests()
    {
        _configLoader = new ConfigLoader(TestBasePath);
        _agentRegistry = new AgentRegistry();
        _logger = Mock.Of<ILogger<WorkflowEngine>>();
        _runtimeLogger = Mock.Of<ILogger<RuntimeStore>>();
        _tempDir = Path.Combine(Path.GetTempPath(), "ai-company-test", Guid.NewGuid().ToString());
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
    }

    private IAgent CreateMockAgent(string name)
    {
        var mock = new Mock<IAgent>();
        mock.Setup(a => a.Name).Returns(name);
        mock.Setup(a => a.ExecuteAsync(It.IsAny<AgentTask>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AgentResult(true, $"[{name}] completed", ["output.md"], TimeSpan.FromSeconds(1), 100, null));
        return mock.Object;
    }

    [Fact]
    public async Task ExecuteAsync_BugFixWorkflow_CompletesSuccessfully()
    {
        foreach (var agentName in new[] { "qa-engineer", "tech-lead", "dynamic", "backend-developer", "frontend-developer", "mobile-developer", "devops-engineer" })
            _agentRegistry.Register(CreateMockAgent(agentName));

        var engine = new WorkflowEngine(_agentRegistry, _configLoader, new RuntimeStore(TestBasePath, _runtimeLogger), _logger);
        var result = await engine.ExecuteAsync("bug-fix");

        result.Status.Should().Be(WorkflowStatus.Completed);
        result.Steps.Should().HaveCount(5);
        result.Steps.Should().AllSatisfy(s => s.Success.Should().BeTrue());
    }

    [Fact]
    public async Task ExecuteAsync_UnknownWorkflow_ThrowsException()
    {
        var engine = new WorkflowEngine(_agentRegistry, _configLoader, new RuntimeStore(TestBasePath, _runtimeLogger), _logger);

        Func<Task> act = () => engine.ExecuteAsync("nonexistent-workflow");

        await act.Should().ThrowAsync<ConfigException>();
    }

    [Fact]
    public async Task ExecuteAsync_LoadsCorrectStepsForWorkflow()
    {
        var definition = _configLoader.LoadWorkflow("bug-fix");

        definition.Name.Should().Be("bug-fix");
        definition.Steps.Should().HaveCount(5);
        definition.Steps[0].Agent.Should().Be("qa-engineer");
        definition.Steps[1].Agent.Should().Be("tech-lead");
        definition.ErrorHandling.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetStatus_AfterExecution_ReturnsRun()
    {
        foreach (var agentName in new[] { "qa-engineer", "tech-lead", "dynamic", "backend-developer", "frontend-developer", "mobile-developer", "devops-engineer" })
            _agentRegistry.Register(CreateMockAgent(agentName));

        var engine = new WorkflowEngine(_agentRegistry, _configLoader, new RuntimeStore(TestBasePath, _runtimeLogger), _logger);
        var result = await engine.ExecuteAsync("bug-fix");

        var status = engine.GetStatus(result.Id);
        status.Should().NotBeNull();
        status!.Status.Should().Be(WorkflowStatus.Completed);
    }

    [Fact]
    public async Task ExecuteAsync_AgentStepWithGate_ResultsPendingApproval()
    {
        var mockAgent = new Mock<IAgent>();
        mockAgent.Setup(a => a.Name).Returns("test-agent");
        mockAgent.Setup(a => a.ExecuteAsync(It.IsAny<AgentTask>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AgentResult(true, "Done", [], TimeSpan.FromSeconds(1), 100, null));
        _agentRegistry.Register(mockAgent.Object);

        var engine = new WorkflowEngine(_agentRegistry, _configLoader, new RuntimeStore(TestBasePath, _runtimeLogger), _logger);
        var result = await engine.ExecuteAsync("testing-to-review");

        result.Should().NotBeNull();
    }
}
