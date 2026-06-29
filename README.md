# Harness AI Software Company

An autonomous multi-agent software development platform powered by specialized AI agents. This repository defines the complete operating system for an AI-driven engineering organization, including agent definitions, workflows, prompts, shared memory, and templates.

## Architecture

```
                           ┌─────────────┐
                           │    CEO      │
                           │  (Strategy) │
                           └──────┬──────┘
                                  │
                           ┌──────┴──────┐
                           │    PM       │
                           │  (Product)  │
                           └──────┬──────┘
                                  │
                     ┌────────────┴────────────┐
                     │                         │
              ┌──────┴──────┐          ┌───────┴───────┐
              │     BA      │          │  Solution     │
              │ (Stories)   │          │  Architect    │
              └──────┬──────┘          └───────┬───────┘
                     │                         │
              ┌──────┴──────┐                  │
              │  Tech Lead  │◄─────────────────┘
              │ (Planning)  │
              └──────┬──────┘
                     │
         ┌───────────┼───────────┐
         │           │           │
   ┌─────┴─────┐ ┌──┴───┐ ┌────┴────┐
   │  Backend  │ │ Front│ │ Mobile  │
   │   Dev     │ │  Dev │ │  Dev    │
   └─────┬─────┘ └──┬───┘ └────┬────┘
         │          │          │
         └──────────┼──────────┘
                    │
              ┌─────┴─────┐
              │  QA Eng   │
              └─────┬─────┘
                    │
              ┌─────┴─────┐
              │  Security │
              └─────┬─────┘
                    │
              ┌─────┴─────┐
              │  DevOps   │
              └─────┬─────┘
                    │
              ┌─────┴─────┐
              │   Docs    │
              │  Writer   │
              └───────────┘
```

## Quick Start

```bash
# Clone the repository
git clone https://github.com/your-org/ai-software-company.git
cd ai-software-company

# Initialize the AI platform
ai-company init

# Run a workflow
ai-company run idea-to-prd --input "Build a payment gateway"

# Chat with an agent
ai-company agent backend-developer "Implement the order API endpoint"
```

## Repository Structure

```
.ai/               # AI platform configuration
  agents/          # 13 specialized AI agents
  workflows/       # SDLC workflow definitions
  prompts/         # Reusable prompt library
  memory/          # Shared memory/context stores
  templates/       # Artifact templates
  schemas/         # Validation schemas
  tools/           # Tool definitions
docs/              # Documentation
src/               # Implementation
tests/             # Test suites
samples/           # Example outputs
scripts/           # Utility scripts
```

## License

MIT
