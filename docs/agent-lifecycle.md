# Agent Lifecycle

## Agent Lifecycle States

```
┌──────────┐     ┌──────────┐     ┌──────────┐     ┌──────────┐
│  IDLE    │────▶│ ASSIGNED │────▶│ WORKING  │────▶│ REVIEW   │
│          │     │          │     │          │     │          │
└──────────┘     └──────────┘     └──────────┘     └──────────┘
                                                      │
                                                      ▼
                                               ┌──────────┐     ┌──────────┐
                                               │ APPROVED │────▶│ COMPLETED│
                                               │          │     │          │
                                               └──────────┘     └──────────┘
                                                      │
                                                      ▼
                                               ┌──────────┐
                                               │ REJECTED │────▶ Return to WORKING
                                               └──────────┘
```

### State Descriptions

| State | Description | Max Duration |
|-------|-------------|--------------|
| IDLE | Agent available, no active task | Unlimited |
| ASSIGNED | Task received, reviewing inputs | 5 minutes |
| WORKING | Actively processing the task | Configurable (default: 30 min) |
| REVIEW | Output awaiting approval | Configurable |
| APPROVED | Output accepted | Immediate |
| REJECTED | Output rejected, needs revision | Immediate |
| COMPLETED | Task finished successfully | Archive after 24 hours |

## Agent Initialization

When an agent receives a task:

1. **Load context**: Read relevant shared memory files
2. **Read config**: Load agent-specific config.yaml
3. **Build prompt**: Combine system.md with task-specific prompt template
4. **Set constraints**: Apply rules from rules.md
5. **Begin processing**: Execute within allocated tools and permissions

## Task Assignment

Tasks are assigned to agents via the workflow engine or direct invocation:

```yaml
# Via workflow step
- id: implement_conversion
  agent: backend-developer
  action: implement
  input: task_definition.md
  output: implementation_complete.md

# Via direct agent call
ai-company agent backend-developer "Implement currency conversion API"
```

## Memory Access Pattern

Agents follow this pattern when accessing shared memory:

1. **Read**: Load relevant memory files on task start
2. **Reference**: Use memory as context during processing
3. **Write**: Update memory when producing new information
4. **Commit**: Memory changes are persisted and versioned

## Agent Communication

Agents communicate through:

1. **Workflow artifacts**: Output of one step is input to another
2. **Shared memory**: `.ai/memory/*` files read/written by multiple agents
3. **Direct invocation**: One agent calls another via `llm-call` tool
4. **Notifications**: Events broadcast to subscribed agents

## Error Recovery

When an agent encounters an error:

1. **Retry**: Automatic retry (3 attempts with exponential backoff)
2. **Escalate**: If retries exhausted, escalate to Tech Lead or next agent in chain
3. **Fallback**: Use degraded mode (e.g., serve cached data if API is down)
4. **Log**: Document error and recovery action for post-mortem
