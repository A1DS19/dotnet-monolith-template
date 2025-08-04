-- =====================================================
-- DMT Database Complete Setup Script
-- Run this script in any SQL Server client
-- =====================================================

PRINT '=== DMT Database Setup Started ===';
PRINT '';

-- =====================================================
-- Step 1: Create Database
-- =====================================================
PRINT '1. Creating Database...';

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DMT')
BEGIN
    CREATE DATABASE [DMT];
    PRINT 'Database DMT created successfully.';
END
ELSE
BEGIN
    PRINT 'Database DMT already exists.';
END
GO

USE [DMT];
GO

-- =====================================================
-- Step 2: Create Tables
-- =====================================================
PRINT '2. Creating Tables...';

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

-- =====================================================
-- Step 3: Seed Data
-- =====================================================
PRINT '3. Seeding Initial Data...';

-- Seed initial data
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products])
BEGIN
    INSERT INTO [dbo].[Products] ([Name], [Price]) VALUES
    ('Laptop Pro 15"', 1299.99),
    ('Wireless Mouse', 29.99),
    ('Mechanical Keyboard', 149.99),
    ('USB-C Hub', 79.99),
    ('External Monitor 27"', 349.99),
    ('Noise Cancelling Headphones', 199.99),
    ('Webcam HD', 89.99),
    ('Desk Lamp LED', 45.99),
    ('Ergonomic Chair', 299.99),
    ('Standing Desk', 499.99);
    
    PRINT 'Sample data inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Products table already contains data.';
END
GO

-- Display inserted data
PRINT '4. Displaying Products...';
SELECT 
    [ID],
    [Name],
    [Price],
    [CreatedAt],
    [UpdatedAt]
FROM [dbo].[Products]
ORDER BY [ID];
GO

PRINT '';
PRINT '=== DMT Database Setup Completed Successfully ===';
GO
