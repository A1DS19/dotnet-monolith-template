-- Create Database
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
