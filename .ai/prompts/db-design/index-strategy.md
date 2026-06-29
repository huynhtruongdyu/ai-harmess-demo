# Index Strategy Prompt

## Context
Design an indexing strategy for {table_name} based on expected query patterns.

## Table Details
- **Table**: {table_name}
- **Estimated Rows**: {estimated_rows}
- **Growth Rate**: {growth_rate} (rows/month)
- **Write Frequency**: {write_frequency} (inserts/updates/deletes per minute)

## Query Patterns
```sql
{query_patterns}
```

## Index Design Principles

### Clustered Index
- Primary key (usually auto-increment ID)
- Consider GUID keys carefully (fragmentation, page splits)

### Non-Clustered Indexes
- Foreign key columns (to support JOIN operations)
- Columns in WHERE, JOIN, ORDER BY, GROUP BY clauses
- High-selectivity columns first in composite indexes
- Include columns in INCLUDE clause (not key columns) for covering indexes

### Index Maintenance
- Monitor fragmentation > 30% → rebuild
- Monitor fragmentation > 10% → reorganize
- Remove unused indexes (check sys.dm_db_index_usage_stats)
- Avoid redundant indexes (same leading column)

## Index Recommendation Template
```sql
-- High priority: directly addresses frequent query patterns
CREATE NONCLUSTERED INDEX IX_{Table}_{Columns}
    ON {Table} ({key_columns})
    INCLUDE ({included_columns})
    WHERE {filter_predicate} -- filtered index, if applicable
    WITH (FILLFACTOR = 90, ONLINE = ON);

-- Medium priority: improves secondary query patterns
-- (create after high-priority indexes evaluated)
```

## Expected Impact
| Index | Query Improvement | Write Overhead | Disk Space |
|-------|-------------------|----------------|------------|
| {idx} | {improvement} | {overhead} | {space} |
