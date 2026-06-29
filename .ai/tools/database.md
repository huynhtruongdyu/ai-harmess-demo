# Database Operations Tool

## Name
`database`

## Description
Executes database queries, migrations, and management operations.

## Input Schema
```json
{
  "type": "object",
  "required": ["operation"],
  "properties": {
    "operation": {
      "type": "string",
      "enum": ["query", "execute", "migrate", "rollback", "seed", "backup", "restore", "status"]
    },
    "connection_string_ref": { "type": "string", "description": "Reference to connection string in secret store" },
    "sql": { "type": "string", "description": "SQL query or command (for query/execute)" },
    "migration_name": { "type": "string", "description": "Migration identifier (for migrate/rollback)" },
    "environment": { "type": "string", "enum": ["dev", "staging", "production"] }
  }
}
```

## Output Schema
```json
{
  "type": "object",
  "properties": {
    "success": { "type": "boolean" },
    "rows_affected": { "type": "integer" },
    "results": { "type": "array" },
    "execution_time_ms": { "type": "integer" },
    "error": { "type": "string" }
  }
}
```

## Side Effects
- `database_write`: Modifies database state
- Confirmation required for: any write operation on staging/production, schema changes, data deletion

## Allowed Agents
- Backend Developer (dev database only)
- DevOps Engineer (all environments, with approval)
- QA Engineer (test data setup/teardown)

## Access Rules
| Environment | Read | Write | Schema Change |
|-------------|------|-------|---------------|
| Dev | Allowed | Allowed | Allowed |
| Staging | Allowed | DevOps only | DevOps only |
| Production | DevOps only | DevOps + approval | DevOps + approval |

## Safety Measures
- SELECT statements automatically limited to 1000 rows
- UPDATE/DELETE without WHERE clause is BLOCKED
- Production queries run with statement_timeout of 30 seconds
- All production queries are logged for audit
- Connection strings never exposed in logs or output
