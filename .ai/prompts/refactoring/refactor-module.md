# Refactor Module Prompt

## Context
Refactor the {module_name} module to improve {improvement_area} (maintainability/performance/testability) without changing behavior.

## Current State
- **Module**: {module_path}
- **Lines of Code**: {loc}
- **Cyclomatic Complexity**: {complexity}
- **Code Coverage**: {coverage}%
- **Known Issues**: {known_issues}

## Refactoring Goals
- {goal_1}
- {goal_2}
- {goal_3}

## Constraints
1. **Behavior Preservation**: All existing tests must pass. No functionality changes.
2. **Incremental**: Each step should be independently mergeable.
3. **No new features**: Pure refactoring only — no feature additions.
4. **API compatibility**: Public interfaces must remain backward compatible.

## Approach
1. **Identify seams**: Find natural boundaries for extraction
2. **Extract interfaces**: Define interfaces for dependencies
3. **Extract classes/services**: Break large classes into focused ones
4. **Improve naming**: Clarify methods, variables, and types
5. **Add missing tests**: For untested logic (characterization tests)
6. **Remove dead code**: Eliminate unused methods, parameters, and variables

## Verification Checklist
- [ ] All existing tests pass
- [ ] No behavior changes (characterization tests confirm)
- [ ] Code coverage maintained or improved
- [ ] Cyclomatic complexity reduced
- [ ] No new warnings or lint errors
- [ ] Performance benchmark shows no regression
- [ ] PR clearly separated into: refactoring only, no feature changes
