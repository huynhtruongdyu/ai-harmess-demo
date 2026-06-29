using AiCompany.Configuration;
using FluentValidation;

namespace AiCompany.Validation;

public class AgentValidator : AbstractValidator<AgentConfig>
{
    public AgentValidator()
    {
        RuleFor(x => x.Agent.Name)
            .NotEmpty().WithMessage("Agent name is required")
            .Matches("^[a-z0-9-]+$").WithMessage("Agent name must be lowercase, alphanumeric with hyphens");

        RuleFor(x => x.Agent.Model)
            .NotEmpty().WithMessage("Model is required");

        RuleFor(x => x.Agent.Temperature)
            .InclusiveBetween(0.0, 2.0).WithMessage("Temperature must be between 0.0 and 2.0");

        RuleFor(x => x.Agent.MaxTokens)
            .InclusiveBetween(500, 8000).WithMessage("MaxTokens must be between 500 and 8000");

        RuleFor(x => x.TimeoutMinutes)
            .InclusiveBetween(5, 120).WithMessage("Timeout must be between 5 and 120 minutes");
    }
}

public class WorkflowValidator : AbstractValidator<WorkflowDefinition>
{
    public WorkflowValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Workflow name is required")
            .Matches("^[a-z0-9-]+$").WithMessage("Workflow name must be lowercase, alphanumeric with hyphens");

        RuleFor(x => x.Trigger.Event)
            .NotEmpty().WithMessage("Trigger event is required");

        RuleFor(x => x.Steps)
            .NotEmpty().WithMessage("At least one step is required");

        RuleForEach(x => x.Steps).ChildRules(step =>
        {
            step.RuleFor(s => s.Id).NotEmpty();
            step.RuleFor(s => s.Agent).NotEmpty();
            step.RuleFor(s => s.Action).NotEmpty();
        });
    }
}
