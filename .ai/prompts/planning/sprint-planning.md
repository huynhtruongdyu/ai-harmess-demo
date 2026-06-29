# Sprint Planning Prompt

## Context
You are planning Sprint {sprint_number} for the {project_name} team.

## Inputs
- Current sprint board: `.ai/memory/current/sprint-board.json`
- Team velocity (3-sprint rolling average): {velocity} points
- Team capacity: {team_capacity} developer-days
- Backlog (prioritized): `.ai/memory/current/backlog.md`
- Previous sprint retrospective: `.ai/memory/current/retro-{previous_sprint}.md`
- Open P1/P2 bugs: `.ai/memory/open-issues.md`

## Instructions
1. Review team capacity and velocity
2. Select top-priority stories from backlog that fit within capacity
3. Ensure at least 15% capacity reserved for tech debt and unplanned work
4. Break selected stories into tasks (4-16 hours each)
5. Identify dependencies and sequence tasks
6. Assign tasks to developers based on expertise and workload

## Output Format
```json
{
  "sprint": "{sprint_number}",
  "total_capacity_points": {capacity},
  "planned_points": {planned},
  "stories": [
    {
      "id": "ST-{number}",
      "title": "{story_title}",
      "points": {estimate},
      "tasks": [
        {
          "id": "T-{number}",
          "title": "{task_title}",
          "assignee": "{developer}",
          "estimated_hours": {hours}
        }
      ]
    }
  ],
  "capacity_buffer_percent": 15,
  "risks": ["{risk_description}"]
}
```

## Constraints
- Do not exceed team capacity
- Maintain knowledge distribution (no single point of failure)
- Include time for testing, code review, and documentation
- Flag any stories that need refinement before they can be estimated
