# Debug Error Prompt

## Context
You are debugging an error in the {component} service.

## Error Details
- **Error Message**: {error_message}
- **Stack Trace**: {stack_trace}
- **Environment**: {environment} ({region})
- **Timestamp**: {timestamp}
- **Correlation ID**: {correlation_id}
- **User Impact**: {user_impact}
- **Frequency**: {frequency} (e.g., "3 times in last hour")

## Relevant Logs
```logs
{relevant_logs}
```

## Recent Changes
- **Recently deployed version**: {deployed_version}
- **Recent commits**: {recent_commits}
- **Related PRs**: {related_prs}

## Instructions
1. **Understand the error**: Analyze the stack trace and logs to understand what failed
2. **Identify root cause**: Trace through the code path to find the root cause
3. **Determine impact**: How many users/requests are affected?
4. **Propose fix**: What change is needed to fix this?
5. **Prevent recurrence**: What monitoring or tests can prevent this in the future?

## Analysis Template
### Root Cause
{root_cause_analysis}

### Fix
```diff
- {current_code}
+ {fixed_code}
```

### Prevention
- [ ] Add unit test covering this scenario
- [ ] Add monitoring alert for this failure mode
- [ ] Add input validation if applicable
- [ ] Review similar code paths for same issue

### Severity Assessment
{severity_justification}
