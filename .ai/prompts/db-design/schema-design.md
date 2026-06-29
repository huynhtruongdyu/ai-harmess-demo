# Database Schema Design Prompt

## Context
Design a database schema for {feature_name}.

## Requirements
- **Domain Entities**: {entities}
- **Relationships**: {relationships_description}
- **Estimated Data Volume**: {volume}
- **Query Patterns**: {query_patterns}
- **Performance Requirements**: {perf_requirements}

## Design Guidelines

### Naming Conventions
- Tables: PascalCase, plural nouns (`Orders`, `OrderItems`, `CustomerAddresses`)
- Columns: PascalCase (`CreatedAt`, `CustomerId`, `EmailAddress`)
- Primary keys: `Id` (auto-increment INT or GUID)
- Foreign keys: `{ReferencedTable}Id` (`CustomerId`)
- Indexes: `IX_{TableName}_{ColumnName}`
- Unique constraints: `UQ_{TableName}_{ColumnName}`

### Column Design
- Define explicit data types (no NVARCHAR(MAX) without reason)
- Set appropriate max lengths for string columns
- Use appropriate numeric types (DECIMAL for money, INT for counts)
- Nullable columns only where semantically correct
- Default values for non-nullable columns
- Audit columns on every table: `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`
- Soft delete flag: `IsDeleted` (BIT, default 0) where appropriate

### Index Strategy
- Primary key = clustered index
- Foreign keys = non-clustered indexes
- Query-filtered columns = non-clustered indexes
- Covering indexes for frequent queries
- Avoid over-indexing (write performance impact)

## Schema Template
```sql
CREATE TABLE {TableName} (
    Id              {id_type}       NOT NULL IDENTITY(1,1),
    {column_name}   {data_type}     {nullability} {constraints},
    CreatedAt       DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy       NVARCHAR(100)   NOT NULL,
    UpdatedAt       DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy       NVARCHAR(100)   NOT NULL,
    IsDeleted       BIT             NOT NULL DEFAULT 0,

    CONSTRAINT PK_{TableName} PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_{TableName}_{ParentTable}
        FOREIGN KEY ({ParentTable}Id) REFERENCES {ParentTable}(Id)
);

CREATE INDEX IX_{TableName}_{ColumnName}
    ON {TableName} ({ColumnName})
    INCLUDE ({included_columns});
```
