# Workflow Engine

## Overview

The workflow engine orchestrates multi-step processes by routing tasks between agents. Workflows are defined as YAML files in `.ai/workflows/`.

## Workflow Structure

```yaml
name: workflow-name
description: "What this workflow does"
version: 1.0.0

trigger:
  event: event_name          # What starts this workflow
  artifact: path/to/file     # Initial input artifact

steps:
  - id: step_id              # Unique step identifier
    agent: agent-name        # Which agent handles this step
    action: action_name      # What the agent should do
    input: input_path        # Input artifact(s)
    output: output_path      # Output artifact
    prompt_template: |       # Optional prompt override
      Instructions for the agent...
    gate:                    # Optional approval gate
      requires_approval: true
      approval_criteria:
        - "Criterion 1"
    parallel: false          # Run steps in parallel (for multiple agents)

error_handling:
  - if: condition
    action: action_name
    message: "Error message"
```

## Trigger Types

| Event | Description |
|-------|-------------|
| `new_idea` | New idea submitted to `.ai/memory/current/idea.md` |
| `prd.approved` | PRD receives executive approval |
| `stories.validated` | User stories validated by BA and Architect |
| `architecture.approved` | Architecture design approved |
| `sprint.started` | Sprint begins |
| `pr.merged` | Pull request merged |
| `qa.signed_off` | QA verification complete |
| `review.approved` | Code review approved |
| `deployment.complete` | Deployment finished |
| `bug.reported` | Bug filed in open-issues |
| `production_incident.p0_p1` | Critical production issue |

## Step Types

### Standard Step
Single agent processes input and produces output.

### Parallel Step
Multiple agents work simultaneously on independent tasks.

```yaml
- id: implement_features
  agent: dynamic
  parallel: true
  target_agents:
    - backend-developer
    - frontend-developer
```

### Gate Step
Requires approval before proceeding to next step.

```yaml
- id: review_prd
  agent: ceo
  gate:
    requires_approval: true
    approval_criteria:
      - Strategic alignment confirmed
      - Success metrics measurable
```

### Conditional Step
Executes based on the outcome of a previous step.

```yaml
- id: verify_fix
  agent: qa-engineer
  condition: "bug.severity == P0 || bug.severity == P1"
```

## Error Handling

```yaml
error_handling:
  - if: timeout > 3600
    action: escalate
    to: human
    message: "Workflow exceeded time limit"

  - if: tests_fail
    action: return_to_dev
    message: "Tests failing. Fix and resubmit."

  - if: capacity_exceeded
    action: negotiate_scope
    between: [tech-lead, product-manager]
```

## Custom Workflows

To create a custom workflow:

1. Create a YAML file in `.ai/workflows/your-workflow.yaml`
2. Define the trigger event
3. List steps with agent assignments
4. Configure error handling
5. Run with: `ai-company run your-workflow`

## Workflow Best Practices

- Each step should have a single responsibility
- Keep steps small enough that failure recovery is simple
- Use gates for irreversible decisions (production deploy, major scope changes)
- Document assumptions and preconditions in step descriptions
- Test workflows with dry-run mode before execution
- Monitor workflow execution time and set appropriate timeouts
