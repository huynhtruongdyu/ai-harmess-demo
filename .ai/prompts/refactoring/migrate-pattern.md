# Migrate Pattern Prompt

## Context
Migrate the codebase from {old_pattern} to {new_pattern} across the {scope} scope.

## Migration Scope
- **Files to migrate**: {file_count}
- **Pattern**: {old_pattern} → {new_pattern}
- **Why**: {rationale}
- **Related ADR**: {adr_reference}

## Migration Strategy
**Approach**: {big_bang / incremental / strangler_fig / parallel_run}

### Incremental Migration Steps
1. **Step 1**: {step_1_description} — Backward compatible? Yes
2. **Step 2**: {step_2_description} — Backward compatible? Yes
3. **Step 3**: {step_3_description} — Backward compatible? Might break callers
4. **Step 4**: {step_4_description} — Remove old implementation

## Migration Template
```diff
- // Old pattern
- {old_code_example}

+ // New pattern
+ {new_code_example}
```

## Verification
- [ ] All existing tests pass after each step
- [ ] Both old and new paths work during migration (dual writes/routes)
- [ ] Performance is not degraded
- [ ] No behavioral changes for end users
- [ ] Old code path removed after migration complete
- [ ] Documentation updated to reflect new pattern
- [ ] Team trained on new pattern (if significant change)

## Rollback Plan
{rollback_plan}
