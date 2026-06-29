using AiCompany.Configuration;
using AiCompany.Validation;
using FluentAssertions;
using Xunit;

namespace AiCompany.Tests.Unit.Core;

public class WorkflowValidatorTests
{
    private readonly WorkflowValidator _validator = new();

    [Fact]
    public void ValidWorkflow_PassesValidation()
    {
        var workflow = new WorkflowDefinition
        {
            Name = "test-workflow",
            Description = "A test workflow",
            Trigger = new WorkflowTrigger { Event = "test.event" },
            Steps = new List<WorkflowStep>
            {
                new() { Id = "step-1", Agent = "test-agent", Action = "test" }
            }
        };

        var result = _validator.Validate(workflow);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void WorkflowWithoutName_FailsValidation()
    {
        var workflow = new WorkflowDefinition
        {
            Name = "",
            Trigger = new WorkflowTrigger { Event = "test" },
            Steps = new List<WorkflowStep>
            {
                new() { Id = "step-1", Agent = "a", Action = "b" }
            }
        };

        var result = _validator.Validate(workflow);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void WorkflowWithoutSteps_FailsValidation()
    {
        var workflow = new WorkflowDefinition
        {
            Name = "test",
            Trigger = new WorkflowTrigger { Event = "test" },
            Steps = new List<WorkflowStep>()
        };

        var result = _validator.Validate(workflow);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void InvalidWorkflowName_FailsValidation()
    {
        var workflow = new WorkflowDefinition
        {
            Name = "Invalid Workflow Name!",
            Trigger = new WorkflowTrigger { Event = "test" },
            Steps = new List<WorkflowStep>
            {
                new() { Id = "step-1", Agent = "a", Action = "b" }
            }
        };

        var result = _validator.Validate(workflow);

        result.IsValid.Should().BeFalse();
    }
}
