-- Database Schema for {ServiceName}
-- Version: {version}
-- Last Updated: {date}

-- =====================================================
-- Table: {TableName}
-- Description: {table_description}
-- =====================================================
CREATE TABLE {schema}.{TableName} (
    Id                  {id_type}       NOT NULL IDENTITY(1,1),

    -- {group_description}
    {column_name}       {data_type}     {nullability} {constraint},

    -- Audit columns
    CreatedAt           DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    CreatedBy           NVARCHAR(100)   NOT NULL,
    UpdatedAt           DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedBy           NVARCHAR(100)   NOT NULL,
    IsDeleted           BIT             NOT NULL DEFAULT 0,
    DeletedAt           DATETIME2       NULL,
    DeletedBy           NVARCHAR(100)   NULL,

    -- Constraints
    CONSTRAINT PK_{TableName} PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_{TableName}_{ParentTable}
        FOREIGN KEY ({ParentTable}Id) REFERENCES {ParentTable}(Id),
    CONSTRAINT UQ_{TableName}_{Column}
        UNIQUE ({unique_columns}),
    CONSTRAINT CK_{TableName}_{Column}
        CHECK ({check_condition})
);

-- Indexes
CREATE NONCLUSTERED INDEX IX_{TableName}_{Column}
    ON {schema}.{TableName} ({column_name})
    INCLUDE ({included_columns})
    WHERE IsDeleted = 0;

-- =====================================================
-- Type: {TypeName}
-- Description: {type_description}
-- =====================================================
CREATE TYPE {schema}.{TypeName} AS TABLE (
    {column_name} {data_type} {nullability}
);
