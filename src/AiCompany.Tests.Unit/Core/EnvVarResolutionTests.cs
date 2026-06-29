using AiCompany.Configuration;
using FluentAssertions;
using Xunit;

namespace AiCompany.Tests.Unit.Core;

public class EnvVarResolutionTests
{
    [Fact]
    public void Resolve_SimpleVar_ReturnsValue()
    {
        Environment.SetEnvironmentVariable("TEST_VAR", "hello");
        var result = ConfigLoader.ResolveEnvironmentVariables("${TEST_VAR}");
        result.Should().Be("hello");
    }

    [Fact]
    public void Resolve_VarWithDefault_ReturnsValueWhenSet()
    {
        Environment.SetEnvironmentVariable("TEST_VAR2", "world");
        var result = ConfigLoader.ResolveEnvironmentVariables("${TEST_VAR2:-default}");
        result.Should().Be("world");
    }

    [Fact]
    public void Resolve_VarWithDefault_ReturnsDefaultWhenUnset()
    {
        Environment.SetEnvironmentVariable("TEST_VAR3", null);
        var result = ConfigLoader.ResolveEnvironmentVariables("${TEST_VAR3:-fallback}");
        result.Should().Be("fallback");
    }

    [Fact]
    public void Resolve_VarWithError_ThrowsWhenUnset()
    {
        Environment.SetEnvironmentVariable("TEST_VAR4", null);
        Action act = () => ConfigLoader.ResolveEnvironmentVariables("${TEST_VAR4:?Required variable missing}");
        act.Should().Throw<ConfigException>()
            .WithMessage("*Required variable missing*");
    }

    [Fact]
    public void Resolve_MissingVarWithoutDefault_Throws()
    {
        Environment.SetEnvironmentVariable("TEST_VAR5", null);
        Action act = () => ConfigLoader.ResolveEnvironmentVariables("${TEST_VAR5}");
        act.Should().Throw<ConfigException>()
            .WithMessage("*TEST_VAR5*not set*");
    }

    [Fact]
    public void Resolve_MultipleVars_AllResolved()
    {
        Environment.SetEnvironmentVariable("HOST", "localhost");
        Environment.SetEnvironmentVariable("PORT", "8080");
        var result = ConfigLoader.ResolveEnvironmentVariables("${HOST}:${PORT}");
        result.Should().Be("localhost:8080");
    }

    [Fact]
    public void Resolve_NoPlaceholders_Unchanged()
    {
        var result = ConfigLoader.ResolveEnvironmentVariables("plain text without vars");
        result.Should().Be("plain text without vars");
    }

    [Fact]
    public void Resolve_EmptyInput_ReturnsEmpty()
    {
        var result = ConfigLoader.ResolveEnvironmentVariables("");
        result.Should().Be("");
    }
}
