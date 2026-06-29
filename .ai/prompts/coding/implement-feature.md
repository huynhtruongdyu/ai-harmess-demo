# Implement Feature Prompt

## Context
Implement the feature described in the user story below.

## User Story
**ID**: {story_id}
**Title**: {story_title}
**Description**: {story_description}
**Acceptance Criteria**:
```gherkin
{acceptance_criteria}
```

## Architecture Context
- **Design Document**: {architecture_reference}
- **Related ADRs**: {adr_references}
- **Existing Code Patterns**: See `src/` for similar implementations

## Implementation Guidelines
1. Start by understanding the full scope: read the story, AC, and related docs
2. Write tests first (TDD approach) — or at minimum, tests alongside implementation
3. Handle all states: loading, success, empty, error, edge cases
4. Consider security implications: input validation, authorization, data access
5. Consider performance: N+1 queries, caching opportunities, async I/O
6. Add necessary logging for observability
7. Update documentation if user-facing behavior changes

## Definition of Done
- [ ] All acceptance criteria verified
- [ ] Unit tests written and passing
- [ ] Integration tests written and passing
- [ ] No lint or type errors
- [ ] Code coverage >= 80%
- [ ] No security vulnerabilities introduced
- [ ] Error handling covers all known failure modes
- [ ] Edge cases handled (null inputs, empty lists, timeouts, etc.)
- [ ] PR created with description linking to the story
