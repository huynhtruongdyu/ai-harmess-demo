# Code Review Prompt

## Context
Review the following pull request for quality, correctness, and adherence to standards.

## PR Details
- **PR Number**: {pr_number}
- **Title**: {pr_title}
- **Description**: {pr_description}
- **Author**: {author}
- **Files Changed**: {files_changed}
- **Related Story**: {story_reference}

## Diff
```diff
{pr_diff}
```

## Review Instructions
Review the code for:

### 1. Correctness
- Does the code implement the acceptance criteria?
- Are there logical errors or edge cases not handled?
- Do the tests actually test the right things?

### 2. Architecture & Design
- Does the code follow Clean Architecture / project patterns?
- Is there appropriate separation of concerns?
- Are there SOLID principle violations?

### 3. Security
- OWASP Top 10: Injection, XSS, Broken Auth, etc.
- Are secrets exposed? Input validated?
- Is authorization enforced server-side?

### 4. Performance
- N+1 queries, unnecessary allocations, sync-over-async?
- Cache opportunities missed?
- Large payloads without pagination?

### 5. Readability & Maintainability
- Clear naming, reasonable method length, comments where needed?
- Magic numbers or strings without constants?
- Complex logic without explanation?

### 6. Testing
- Are tests meaningful (not just coverage padding)?
- Do tests cover edge cases and error paths?
- Are integration tests testing real behavior?

## Review Response Format
```
📋 Summary: {brief_summary}

❌ BLOCKING (must fix):
- {file}:{line} — {issue} — [{fix_suggestion}]

💡 SUGGESTIONS (nice to improve):
- {file}:{line} — {suggestion}

❓ QUESTIONS:
- {question}

✅ POSITIVE NOTES:
- {good_aspect}

DECISION: {approve / request_changes / comment}
```
