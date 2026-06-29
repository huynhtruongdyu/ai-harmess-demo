# Run Commands Tool

## Name
`run-commands`

## Description
Executes shell commands, build scripts, test runners, and linters in a sandboxed environment.

## Input Schema
```json
{
  "type": "object",
  "required": ["command"],
  "properties": {
    "command": { "type": "string", "description": "Command to execute" },
    "working_directory": { "type": "string", "description": "Working directory (default: repo root)" },
    "timeout_seconds": { "type": "integer", "description": "Timeout in seconds", "default": 120 },
    "environment_vars": { "type": "object", "description": "Additional environment variables" }
  }
}
```

## Output Schema
```json
{
  "type": "object",
  "properties": {
    "success": { "type": "boolean" },
    "stdout": { "type": "string" },
    "stderr": { "type": "string" },
    "exit_code": { "type": "integer" },
    "duration_ms": { "type": "integer" }
  }
}
```

## Side Effects
- May modify build artifacts, install packages, modify databases
- Requires confirmation for: package installation, database operations, infrastructure commands

## Allowed Agents
- All developer agents (limited to test/lint/build commands)
- DevOps Engineer (infrastructure commands)
- QA Engineer (test execution only)

## Restricted Commands
| Command | Restriction |
|---------|-------------|
| `rm -rf /*`, `format`, `dd` | BLOCKED |
| `npm install`, `dotnet restore` | Allowed (with cache) |
| `kubectl`, `az`, `terraform` | DevOps only |
| Database migrations | Confirmation required |
| Production deployments | DevOps only + approval required |
