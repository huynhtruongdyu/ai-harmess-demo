# Architecture

## System Overview

The Harness AI Software Company is a multi-agent orchestration platform where specialized AI agents collaborate to deliver software. Agents communicate through shared memory and a workflow engine.

## Agent Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Agent Runtime                            │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │  CEO     │  │  PM      │  │  BA      │  │ Architect│   │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘   │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │Tech Lead │  │Backend   │  │Frontend  │  │ Mobile   │   │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘   │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │  QA      │  │ Security │  │ DevOps   │  │ Reviewer │   │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘   │
│  ┌──────────┐                                              │
│  │  Docs    │                                              │
│  └──────────┘                                              │
└─────────────────────────────────────────────────────────────┘
         │                     │                     │
         ▼                     ▼                     ▼
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│  Shared Memory   │  │ Workflow Engine  │  │   Tool System   │
│  (File-based)    │  │ (State Machine)  │  │  (Sandboxed)    │
└─────────────────┘  └─────────────────┘  └─────────────────┘
```

## Communication Model

1. **Direct Invocation**: One agent explicitly calls another via the workflow engine
2. **Shared Memory**: Agents read/write to `.ai/memory/` for persistent context
3. **Events**: Workflow engine emits events that trigger downstream agents
4. **Artifacts**: Agents produce files (PRDs, designs, code) that are consumed by other agents

## Workflow Engine

The workflow engine is a state machine that:

1. **Triggers** on events (e.g., `prd.approved`, `pr.merged`)
2. **Executes steps** sequentially or in parallel
3. **Routes tasks** to the appropriate agent based on capability
4. **Enforces gates** (approval checkpoints) before proceeding
5. **Handles errors** with configurable retry, rollback, and escalation

## Memory System

Memory is stored as markdown files in `.ai/memory/`:

| Store | Purpose | Write Access |
|-------|---------|-------------|
| project-context | High-level project info | CEO, Architect |
| architecture-decisions | ADR log | Architect |
| business-rules | Domain rules | BA |
| coding-standards | Code conventions | Architect |
| glossary | Terminology | BA |
| current-sprint | Sprint tracking | Tech Lead |
| open-issues | Bugs and debt | QA, Devs |
| completed-tasks | History | Tech Lead |

## Tool System

Tools provide agents with capabilities to interact with the environment:

- **File operations**: Read, write, edit files
- **Git operations**: Commit, branch, PR
- **Code analysis**: Lint, typecheck, security scan
- **API calls**: External services (GitHub, Jira, Slack)
- **Database**: Query and migrate
- **LLM call**: Sub-calls to other agents or models

Each tool defines:
- Input/output schema (JSON schema)
- Allowed agents
- Side effects
- Confirmation requirements
- Security restrictions
