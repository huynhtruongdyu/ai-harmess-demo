# Memory System

## Overview

The memory system provides persistent, shared context that all agents can read and write. It serves as the "long-term memory" of the AI software company.

## Storage Model

Memory is stored as Markdown files in `.ai/memory/`:

```
.ai/memory/
├── project-context.md          # Project overview, OKRs, stakeholders
├── architecture-decisions.md   # ADR log
├── business-rules.md           # Domain business rules
├── coding-standards.md         # Code style and conventions
├── glossary.md                 # Domain and technical terminology
├── current-sprint.md           # Active sprint tracking
├── open-issues.md              # Bug and issue tracker
├── completed-tasks.md          # Historical completion records
└── _cache/                     # Ephemeral cache (not committed)
```

## Access Control

Each agent has defined read/write permissions in its `config.yaml`:

```yaml
# backend-developer/config.yaml
memory_access:
  - project-context
  - architecture-decisions
  - coding-standards
  - current-sprint
  - open-issues

memory_write:
  - open-issues       # Can only report issues
```

## Persistence

Memory is persisted via Git:

1. Agents write changes to memory files
2. Changes are committed automatically with descriptive messages
3. History is preserved via Git history
4. Conflicts are resolved with "last writer wins" strategy

## Conflict Resolution

When two agents modify the same memory file:

1. **Auto-merge**: Simple additions (appending) are auto-merged
2. **Last-writer-wins**: Conflicting edits use the most recent change
3. **Conflict flag**: If auto-merge detects conflicting changes, the file is flagged for human review

## Usage Patterns

### Read Before Write
Agents should always read the latest version of memory before writing:

```markdown
1. Read `.ai/memory/architecture-decisions.md`
2. Check if similar decision already exists
3. Append new ADR if unique
4. Commit
```

### Append Predominantly
Memory files use append-heavy patterns:

- ADRs are appended (newest last)
- Business rules are appended with new IDs
- Open issues are appended (resolved items moved to completed)

### Reference by ID
Memory entries should reference each other by ID:

```
See ADR-007 for payment abstraction details.
Business rule BR-012 applies to this calculation.
Related to story ST-101 in current sprint.
```

## Versioning

Memory is implicitly versioned via Git. Each change is tracked:

```bash
git log .ai/memory/architecture-decisions.md
git diff <commit1> <commit2> -- .ai/memory/business-rules.md
```

For external consumers, a `version` field can be embedded in memory entries for explicit version tracking.
