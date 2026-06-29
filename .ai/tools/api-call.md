# API Call Tool

## Name
`api-call`

## Description
Makes HTTP requests to external services (GitHub, Jira, Slack, monitoring tools, etc.).

## Input Schema
```json
{
  "type": "object",
  "required": ["method", "url"],
  "properties": {
    "method": {
      "type": "string",
      "enum": ["GET", "POST", "PUT", "PATCH", "DELETE"]
    },
    "url": { "type": "string", "description": "Full URL (including protocol)" },
    "headers": { "type": "object", "description": "HTTP headers" },
    "body": { "type": "object", "description": "Request body (for POST/PUT/PATCH)" },
    "timeout_seconds": { "type": "integer", "default": 30 },
    "retry_count": { "type": "integer", "default": 2 }
  }
}
```

## Output Schema
```json
{
  "type": "object",
  "properties": {
    "success": { "type": "boolean" },
    "status_code": { "type": "integer" },
    "headers": { "type": "object" },
    "body": { "type": "object" },
    "error": { "type": "string" }
  }
}
```

## Side Effects
- `api_call`: Makes external HTTP requests
- Confirmation required for: write operations (POST/PUT/PATCH/DELETE) on production systems

## Allowed Agents
- DevOps Engineer (infrastructure APIs)
- QA Engineer (test result reporting)
- Tech Lead (GitHub/Jira operations)
- All agents with rate limiting (100 calls/hour total)

## Integrated Services
| Service | Authentication | Scope |
|---------|---------------|-------|
| GitHub API | OAuth token | PRs, issues, repos |
| Jira API | API token | Tickets, sprints |
| Slack API | Bot token | Notifications |
| Azure API | Managed identity | Infrastructure |
| Datadog API | API + App keys | Monitoring |
