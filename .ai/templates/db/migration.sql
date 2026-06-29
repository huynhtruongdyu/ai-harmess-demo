-- =====================================================
-- Migration: {MigrationName}
-- Description: {migration_description}
-- Author: {author}
-- Date: {date}
-- Ticket: {ticket_reference}
-- =====================================================

BEGIN TRANSACTION;

-- Step 1: Add new columns
-- {rationale_for_change}

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'{schema}.{table}')
    AND name = N'{column_name}'
)
BEGIN
    ALTER TABLE {schema}.{table}
    ADD {column_name} {data_type} {nullable}
        CONSTRAINT DF_{table}_{column} DEFAULT {default_value};

    PRINT 'Column {column_name} added to {table}';
END

-- Step 2: Create indexes
-- {index_rationale}

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = N'IX_{table}_{column}'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_{table}_{column}
    ON {schema}.{table} ({column_name})
    INCLUDE ({included_columns});

    PRINT 'Index IX_{table}_{column} created';
END

-- Step 3: Data migration (if needed)
-- {data_migration_description}

-- UPDATE {schema}.{table}
-- SET {new_column} = {derived_value}
-- WHERE {condition};

-- Verify
-- SELECT COUNT(*) AS MigratedCount FROM {schema}.{table}
-- WHERE {new_column} IS NOT NULL;

COMMIT TRANSACTION;
GO
