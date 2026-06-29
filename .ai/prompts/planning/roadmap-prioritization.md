# Roadmap Prioritization Prompt

## Context
Prioritize the product roadmap initiatives based on business value, strategic alignment, and engineering effort.

## Inputs
- Strategic OKRs: `.ai/memory/project-context.md`
- Feature request queue: `.ai/memory/current/feature-requests/`
- Technical debt assessment: `.ai/memory/open-issues.md`
- Market/competitive analysis: {competitive_data}
- Customer feedback summary: {feedback_data}
- Engineering capacity forecast: {capacity_forecast}

## Instructions
1. Score each initiative using RICE framework:
   - **Reach**: How many users will this impact in a given period?
   - **Impact**: How significantly will this affect individual users?
   - **Confidence**: How confident are we in our estimates?
   - **Effort**: How much engineering time is required?

2. Categorize initiatives:
   - **Now** (current quarter): High priority, must-do
   - **Next** (next quarter): Important, schedule next
   - **Later** (future): Valuable but not urgent
   - **Icebox**: Nice-to-have, no immediate plans

## Output Format
```markdown
| Initiative | RICE Score | Category | Dependencies | Notes |
|------------|-----------|----------|--------------|-------|
| {feature}  | {score}   | {now/next/later/icebox} | {deps} | {notes} |
```

## Constraints
- Balance innovation (new features) with maintenance (tech debt, bugs)
- At least 20% of capacity reserved for non-feature work
- Consider dependencies between initiatives
- Align with quarterly OKRs
