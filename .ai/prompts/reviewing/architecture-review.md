# Architecture Review Prompt

## Context
Review the proposed architecture change for alignment with existing patterns and best practices.

## Inputs
- **Design Document**: {design_doc}
- **Proposed ADR**: {adr_draft}
- **Existing ADRs**: .ai/memory/architecture-decisions.md
- **Affected Services**: {affected_services}
- **Stakeholders**: {stakeholders}

## Review Criteria

### Strategic Alignment
- Does this align with the technology strategy?
- Does it leverage existing platform capabilities?
- Is this solving the right problem?

### Architectural Soundness
- Is the design consistent with existing architecture?
- Are the right trade-offs being made?
- Are non-functional requirements addressed (scalability, availability, performance)?
- Are failure modes considered?

### Operability
- Can this be deployed, monitored, and operated by the team?
- Are there clear health checks and metrics?
- Is there a rollback plan?

### Security
- Has a threat model been created?
- Are security controls appropriate for data classification?
- Is the principle of least privilege applied?

### Future-Proofing
- Is the design extensible for future requirements?
- Are there unnecessary abstractions (over-engineering)?
- Are third-party dependencies justified?

## Output Format
```
## Summary
{overall_assessment}

## Compliance with Existing ADRs
- ADR-{number}: {compliant / conflict — explanation}

## Recommendations
1. {recommendation_1}
2. {recommendation_2}

## Concerns
1. {concern_1}
2. {concern_2}

## Decision
{approved / changes_requested / rejected}
```
