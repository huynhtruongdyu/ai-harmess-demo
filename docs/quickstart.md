# Quickstart Guide

## Prerequisites

- Git 2.30+
- .NET 8 SDK (for C# implementation)
- Node.js 18+ (for CLI tool)
- Access to an LLM API (OpenAI, Azure OpenAI, or Anthropic)

## Setup

### 1. Clone the Repository

```bash
git clone https://github.com/your-org/ai-software-company.git
cd ai-software-company
```

### 2. Initialize the Platform

```bash
# Install CLI tools
dotnet tool install --global AiCompany.Cli

# Initialize project
ai-company init
```

### 3. Configure LLM Provider

Copy the example configuration:

```bash
cp .ai/config.example.yaml .ai/config.yaml
```

Edit `.ai/config.yaml` with your LLM API credentials:

```yaml
platform:
  default_provider: "openai"
  default_model: "gpt-4o"

providers:
  openai:
    api_key: "${OPENAI_API_KEY}"  # Set via environment variable
    organization: "your-org"
```

### 4. Set Environment Variables

```bash
export OPENAI_API_KEY="sk-..."
export AI_COMPANY_PROJECT="MerchantOS"
```

## Running Workflows

### Idea to PRD

```bash
# Create a new idea
cat > .ai/memory/current/idea.md << EOF
# Idea: Multi-Currency Checkout
Allow customers to view prices and checkout in their local currency.
Target: EU market expansion, projected 15% conversion increase.
EOF

# Run the workflow
ai-company run idea-to-prd
```

### Chat with an Agent

```bash
# Direct agent interaction
ai-company agent backend-developer "Design the currency conversion API endpoint"

# With context from memory
ai-company agent solution-architect "Review our current architecture" \
  --context .ai/memory/architecture-decisions.md
```

### Sprint Execution

```bash
# Plan sprint from prepared stories
ai-company run architecture-to-tasks

# Execute development tasks
ai-company run tasks-to-development

# Run quality checks
ai-company run development-to-testing
```

## Project Structure

```
ai-software-company/
├── .ai/                    # AI platform configuration
│   ├── agents/             # Agent definitions
│   ├── workflows/          # Workflow pipelines
│   ├── prompts/            # Prompt templates
│   ├── memory/             # Shared memory stores
│   ├── templates/          # Artifact templates
│   ├── schemas/            # Validation schemas
│   ├── tools/              # Tool definitions
│   ├── config.yaml         # Platform config
│   └── routing.yaml        # Task router
├── docs/                   # Documentation
├── src/                    # Implementation
├── tests/                  # Test suites
└── samples/                # Example outputs
```

## Next Steps

1. Read [docs/architecture.md](architecture.md) for system design details
2. Read [docs/agent-lifecycle.md](agent-lifecycle.md) for agent behavior
3. Read [docs/workflow-engine.md](workflow-engine.md) for workflow customization
4. Explore `.ai/agents/` to understand each agent's capabilities
