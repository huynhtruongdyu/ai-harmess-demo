# LLM Call Tool

## Name
`llm-call`

## Description
Invokes another agent or an LLM model for reasoning, analysis, or content generation as a sub-task.

## Input Schema
```json
{
  "type": "object",
  "required": ["prompt"],
  "properties": {
    "prompt": { "type": "string", "description": "The prompt to send" },
    "target_agent": { "type": "string", "description": "Specific agent to route to" },
    "model": { "type": "string", "description": "Model override (default: platform default)" },
    "temperature": { "type": "number", "description": "Temperature override (0.0-2.0)" },
    "max_tokens": { "type": "integer", "description": "Max tokens for response" },
    "context": { "type": "array", "description": "Additional context files or artifacts" }
  }
}
```

## Output Schema
```json
{
  "type": "object",
  "properties": {
    "response": { "type": "string" },
    "model": { "type": "string" },
    "usage": {
      "type": "object",
      "properties": {
        "prompt_tokens": { "type": "integer" },
        "completion_tokens": { "type": "integer" },
        "total_tokens": { "type": "integer" }
      }
    },
    "agent": { "type": "string" }
  }
}
```

## Side Effects
- None (calling LLMs is read-only in terms of system state)
- Costs are tracked per call for monitoring

## Allowed Agents
- All agents (with rate limiting)
- CEO: 50 calls/hour
- Developer agents: 200 calls/hour
- All: 1000 calls/hour total

## Usage Rules
- Route to specific agent when task requires that agent's expertise
- Use general model call when task is generic (analysis, formatting, translation)
- Include relevant context from shared memory to improve response quality
- Temperature: 0.0-0.3 for deterministic tasks, 0.4-0.7 for creative tasks
