using AiCompany.Configuration;
using FluentAssertions;
using Xunit;

namespace AiCompany.Tests.Unit.Core;

public class ConfigLoaderTests
{
    private static readonly string TestBasePath;

    static ConfigLoaderTests()
    {
        var dir = Directory.GetCurrentDirectory();
        while (dir != null && !File.Exists(Path.Combine(dir, ".ai", "config.yaml")))
            dir = Path.GetDirectoryName(dir);
        TestBasePath = dir ?? throw new InvalidOperationException("Could not find .ai directory");
    }

    [Fact]
    public void LoadPlatformConfig_ReadsFromDefaultPath_ReturnsConfig()
    {
        var loader = new ConfigLoader(TestBasePath, resolveEnvVars: false);
        var config = loader.LoadPlatformConfig();

        config.Should().NotBeNull();
        config.Platform.Name.Should().Be("Harness AI Software Company");
        config.Platform.Version.Should().Be("1.0.0");
        config.Platform.DefaultModel.Should().Be("gpt-4o");
    }

    [Fact]
    public void ListAgents_ReturnsAllThirteenAgents()
    {
        var loader = new ConfigLoader(TestBasePath, resolveEnvVars: false);
        var agents = loader.ListAgents();

        agents.Should().HaveCount(13);
        agents.Should().Contain("ceo");
        agents.Should().Contain("backend-developer");
        agents.Should().Contain("frontend-developer");
    }

    [Fact]
    public void ListWorkflows_ReturnsAllSeventeenWorkflows()
    {
        var loader = new ConfigLoader(TestBasePath, resolveEnvVars: false);
        var workflows = loader.ListWorkflows();

        workflows.Should().HaveCount(17);
        workflows.Should().Contain("idea-to-prd");
        workflows.Should().Contain("release");
    }

    [Fact]
    public void LoadAgentConfig_ValidAgent_ReturnsConfig()
    {
        var loader = new ConfigLoader(TestBasePath, resolveEnvVars: false);
        var config = loader.LoadAgentConfig("backend-developer");

        config.Should().NotBeNull();
        config.Agent.Name.Should().Be("backend-developer");
        config.Agent.Model.Should().Be("gpt-4o");
        config.Tools.Should().NotBeEmpty();
    }

    [Fact]
    public void LoadWorkflow_ValidWorkflow_ReturnsDefinition()
    {
        var loader = new ConfigLoader(TestBasePath, resolveEnvVars: false);
        var workflow = loader.LoadWorkflow("idea-to-prd");

        workflow.Should().NotBeNull();
        workflow.Name.Should().Be("idea-to-prd");
        workflow.Steps.Should().NotBeEmpty();
    }

    [Fact]
    public void LoadConfig_NonExistentFile_ThrowsConfigException()
    {
        var loader = new ConfigLoader(TestBasePath, resolveEnvVars: false);

        Action act = () => loader.LoadAgentConfig("nonexistent-agent");

        act.Should().Throw<ConfigException>()
            .WithMessage("*not found*");
    }
}
