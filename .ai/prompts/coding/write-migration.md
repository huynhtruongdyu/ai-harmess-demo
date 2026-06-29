# Database Migration Prompt

## Context
Write a database migration for {change_description}.

## Requirements
- **Migration Description**: {description}
- **Tables Affected**: {tables}
- **Data Migration Required**: {yes/no}
- **Rollback Strategy**: {rollback_strategy}
- **Risk Level**: {low/medium/high}

## Migration Guidelines
1. Create a forward migration script (Up)
2. Create a rollback migration script (Down)
3. If data migration is needed, include it in the same transaction
4. All columns must be nullable initially when adding to existing tables (then ALTER if NOT NULL required)
5. Add indexes for new foreign keys and frequently queried columns
6. Use explicit transaction boundaries
7. Consider locking implications — prefer ONLINE operations where possible

## Migration Template (Up)
```sql
BEGIN TRANSACTION;

-- Add new column
ALTER TABLE {table_name}
ADD {column_name} {data_type} {nullable}
CONSTRAINT {constraint_name} DEFAULT {default_value};

-- Create index
CREATE NONCLUSTERED INDEX IX_{table_name}_{column}
ON {table_name} ({column_name})
INCLUDE ({included_columns});

-- Data migration (if needed)
UPDATE {table_name}
SET {column_name} = {computed_value}
WHERE {column_name} IS NULL;

-- Add NOT NULL constraint (after data migration)
ALTER TABLE {table_name}
ALTER COLUMN {column_name} {data_type} NOT NULL;

COMMIT TRANSACTION;
```

## Migration Template (Down)
```sql
BEGIN TRANSACTION;

DROP INDEX IX_{table_name}_{column} ON {table_name};
ALTER TABLE {table_name} DROP CONSTRAINT {constraint_name};
ALTER TABLE {table_name} DROP COLUMN {column_name};

COMMIT TRANSACTION;
```

## Verification Checklist
- [ ] Forward migration runs successfully against production-sized dataset
- [ ] Rollback migration restores original state
- [ ] Data integrity maintained after migration
- [ ] Performance impact assessed
- [ ] Migration execution time within maintenance window
- [ ] Application compatible with both schema versions (forward/backward compatible)
