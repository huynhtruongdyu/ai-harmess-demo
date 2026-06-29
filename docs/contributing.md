# Contributing

## Adding a New Agent

To add a new agent to the platform:

### 1. Create Agent Files

```bash
mkdir -p .ai/agents/your-agent-name
```

Create the required files:

| File | Purpose | Required |
|------|---------|----------|
| `README.md` | Role description, inputs/outputs, escalation | Yes |
| `system.md` | System prompt with instructions and constraints | Yes |
| `rules.md` | Decision rules, communication rules, escalation rules | Yes |
| `examples.md` | Example conversations and outputs | Yes |
| `config.yaml` | Model, temperature, memory access, tools | Yes |

### 2. Register in Router

Add a routing rule in `.ai/routing.yaml`:

```yaml
- match:
    task_type: "your_agent_type"
    keywords: ["keyword1", "keyword2"]
  target_agent: "your-agent-name"
  priority: 70
```

### 3. Add Workflow Steps

Reference your agent in workflow definitions:

```yaml
- id: your_step
  agent: your-agent-name
  action: your_action
```

### 4. Grant Memory & Tool Access

In `config.yaml`, specify:

```yaml
memory_access:
  - relevant-memory-store

tools:
  - relevant-tools
```

## Adding a New Workflow

1. Create `.ai/workflows/your-workflow.yaml`
2. Follow the workflow schema (see `docs/workflow-engine.md`)
3. Test with: `ai-company run your-workflow --dry-run`

## Adding a New Prompt

1. Create `.ai/prompts/{category}/your-prompt.md`
2. Follow the prompt template pattern with variables in `{curly_braces}`
3. Reference from workflow steps or agent system prompts

## Adding a New Tool

1. Create `.ai/tools/your-tool.md` with input/output schema
2. Implement the tool in `src/tools/`
3. Register in the tool registry
4. Grant access to appropriate agents via their `config.yaml`

## Code Style

See `.ai/memory/coding-standards.md` for complete style guide.

Key points:
- 4-space indentation
- Clear naming conventions per language
- 80%+ test coverage
- No TODOs or commented code in PRs
- Document public APIs

## Testing

```bash
# Run all unit tests
dotnet test tests/unit

# Run integration tests
dotnet test tests/integration

# Run E2E tests
dotnet test tests/e2e

# Validate all agent configs
ai-company validate

# Lint checking
dotnet format --verify-no-changes
```

## PR Process

1. Create feature branch from main
2. Make changes following code style
3. Write/update tests
4. Run lint and tests locally
5. Create PR with description linking to issue/story
6. Address review feedback
7. Merge after approval

## Questions?

- Chat with the Tech Lead agent: `ai-company agent tech-lead "question"`
- Read agent docs: `.ai/agents/*/`
- Read workflow docs: `.ai/workflows/*.yaml`
