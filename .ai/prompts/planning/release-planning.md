# Release Planning Prompt

## Context
Plan the next release for {project_name}. Consider business priorities, engineering capacity, and quality requirements.

## Inputs
- Product roadmap: `.ai/memory/current/roadmap.md`
- Current sprint: `.ai/memory/current-sprint.md`
- Team velocity (quarterly trend): {velocity_data}
- Feature requests in backlog: `.ai/memory/current/backlog.md`
- Technical debt items: `.ai/memory/open-issues.md`

## Instructions
1. Define release scope based on business priorities
2. Estimate total effort for planned features
3. Identify critical path items and dependencies
4. Calculate expected release date based on velocity
5. Define quality gates for this release
6. Identify risks and mitigation strategies

## Output Sections
- **Release Goal**: One-sentence statement of what this release achieves
- **Scope**: Features, improvements, and fixes included
- **Timeline**: Key milestones and expected release date
- **Resources**: Engineers, QA, DevOps effort required
- **Quality Gates**: Criteria that must be met before release
- **Risks**: Top 3 risks and mitigations
- **Out of Scope**: Explicitly excluded from this release
