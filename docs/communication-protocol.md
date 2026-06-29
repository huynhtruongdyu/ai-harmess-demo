# Communication Protocol

## Overview

Agents communicate through a structured protocol that ensures messages are clear, traceable, and actionable.

## Message Format

All inter-agent communication follows this structure:

```markdown
## Message
- **From**: {agent_name}
- **To**: {agent_name / "all" / "system"}
- **Type**: {task / response / escalation / notification / question}
- **Priority**: {P0 / P1 / P2 / P3}
- **ID**: {msg_yyyyMMdd_HHmmss_fff}
- **Context**: {reference to relevant artifacts}

## Body
{message content with clear instructions or information}

## Attachments
- {file_path_1}
- {file_path_2}

## Deadline
{optional: expected completion time}
```

## Communication Types

### Task Assignment
```
From: tech-lead
To: backend-developer
Type: task
Body: Implement the currency conversion API endpoint per the attached spec.
Deadline: 2024-11-22 17:00 UTC
```

### Progress Update
```
From: backend-developer
To: tech-lead
Type: response
Body: Task T-101.5 is 60% complete. Implementation done, writing tests now.
ETA: Tomorrow EOD.
```

### Escalation
```
From: backend-developer
To: tech-lead
Type: escalation
Priority: P1
Body: Blocked on ADR-010 for currency analytics data model.
Need decision to proceed with implementation.
```

### Question
```
From: frontend-developer
To: backend-developer
Type: question
Body: Currency API — should we handle the case where the user
selects the same currency as the base? Return 1.0 rate or skip conversion?
```

### Notification
```
From: system
To: all
Type: notification
Body: Sprint 12 is 50% complete. Current velocity: 42/55 points.
Review blockers and adjust priorities if needed.
```

## Response Expectations

| Type | Expected Response Time | Response Required |
|------|----------------------|-------------------|
| Task (P0) | Immediate | Yes — acknowledgment within 5 min |
| Task (P1) | 1 hour | Yes — acknowledgment within 30 min |
| Task (P2) | 4 hours | Yes — acknowledgment within 2 hours |
| Task (P3) | 24 hours | Yes — acknowledgment within 8 hours |
| Question | 2 hours | Yes |
| Escalation (P0/P1) | 15 minutes | Yes |
| Escalation (P2+) | 2 hours | Yes |
| Notification | N/A | No |

## Channels

| Channel | Use | Persistence |
|---------|-----|-------------|
| Workflow artifacts | Step inputs/outputs | Git-persisted |
| Shared memory | Persistent context | Git-persisted |
| Direct messaging | Task assignment, questions | In-session |
| Events/notifications | Broadcasts | Logged |
