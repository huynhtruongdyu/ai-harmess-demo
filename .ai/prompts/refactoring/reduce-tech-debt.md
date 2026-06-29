# Reduce Technical Debt Prompt

## Context
Address the following technical debt items to improve the health of the codebase.

## Debt Items
```json
{tech_debt_items}
```

## Debt Prioritization Matrix
| Item | Impact | Effort | Risk | Priority Score |
|------|--------|--------|------|---------------|
| {item} | {H/M/L} | {H/M/L} | {H/M/L} | {score} |

## Strategies by Debt Type

### Code Quality Debt
- Extract large classes (> 300 lines)
- Split long methods (> 30 lines)
- Remove duplicate code (DRY violations)
- Add missing error handling
- Improve test coverage in uncovered areas

### Architecture Debt
- Remove circular dependencies
- Extract shared libraries from duplicate code
- Standardize patterns across services
- Remove deprecated endpoints and code paths

### Testing Debt
- Add integration tests for untested services
- Fill gap areas in unit test coverage
- Convert flaky integration tests to reliable ones
- Add performance regression tests

### Dependency Debt
- Update outdated packages (within major version)
- Replace unmaintained libraries with active alternatives
- Remove unused dependencies
- Pin floating version ranges

## Definition of Done
- [ ] Target debt items resolved
- [ ] Existing tests continue to pass
- [ ] Coverage not decreased
- [ ] No new lint/type errors
- [ ] Team notified of changes
- [ ] Documentation updated if interfaces changed
