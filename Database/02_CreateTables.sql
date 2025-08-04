USE [DMT];
GO

-- Create Products Table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Products')
BEGIN
    CREATE TABLE [dbo].[Products] (
        [ID] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(255) NOT NULL,
        [Price] REAL NOT NULL,
        [CreatedAt] DATETIME2 DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 DEFAULT GETUTCDATE()
    );
    
    -- Create index on Name for better query performance
    CREATE INDEX IX_Products_Name ON [dbo].[Products] ([Name]);
    
    PRINT 'Products table created successfully.';
END
ELSE
BEGIN
    PRINT 'Products table already exists.';
END
GO

-- Create trigger to update UpdatedAt timestamp
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Products_UpdateTimestamp')
BEGIN
    EXEC('
    CREATE TRIGGER [dbo].[TR_Products_UpdateTimestamp]
    ON [dbo].[Products]
    AFTER UPDATE
    AS
    BEGIN
        SET NOCOUNT ON;
        UPDATE [dbo].[Products] 
        SET [UpdatedAt] = GETUTCDATE()
        FROM [dbo].[Products] p
        INNER JOIN inserted i ON p.[ID] = i.[ID];
    END
    ');
    
    PRINT 'UpdateTimestamp trigger created successfully.';
END
ELSE
BEGIN
    PRINT 'UpdateTimestamp trigger already exists.';
END
GO
