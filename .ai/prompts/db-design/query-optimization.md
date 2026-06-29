# Query Optimization Prompt

## Context
Optimize the following database query that is experiencing performance issues.

## Problem Query
```sql
{slow_query}
```

## Execution Plan
```xml
{execution_plan_xml_or_text}
```

## Table Statistics
| Table | Row Count | Data Size | Indexes |
|-------|-----------|-----------|---------|
| {table} | {rows} | {size} | {indexes} |

## Current Performance
- **Average Execution Time**: {avg_time}
- **Target Execution Time**: {target_time}
- **Frequency**: {frequency} (e.g., "200 times/minute")
- **Resource Usage**: {resource_usage} (CPU/IO/memory)

## Optimization Techniques to Consider

### Indexing
- Are there missing indexes? (check index seek vs scan in plan)
- Are existing indexes being used? (check key lookup, RID lookup)
- Would a covering index help?
- Would filtered indexes help for specific query patterns?
- Are composite indexes in the right column order?

### Query Rewriting
- Can SELECT * be replaced with specific columns?
- Are JOINs using appropriate columns (indexed, same type)?
- Could subqueries be rewritten as JOINs or CTEs?
- Is there unnecessary data transformation in WHERE clauses?
- Could window functions replace self-joins?

### Schema Changes
- Would a computed column with index help?
- Would table partitioning help for large table scans?
- Could denormalization reduce JOIN complexity?

## Output Format
```markdown
### Root Cause
{identified_root_cause}

### Recommended Fix
{sql_fix}

### Expected Improvement
{expected_performance_gain}

### Risk Assessment
{risks_of_change}

### Verification
- [ ] Query plan shows improvement
- [ ] Regression test on related queries
- [ ] No negative impact on write performance
```
