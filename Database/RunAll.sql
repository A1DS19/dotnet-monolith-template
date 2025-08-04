-- DMT Database Setup Script
-- Run this script to create the complete database structure

PRINT '=== DMT Database Setup Started ===';
PRINT '';

-- Step 1: Create Database
PRINT '1. Creating Database...';
:r 01_CreateDatabase.sql

-- Step 2: Create Tables  
PRINT '';
PRINT '2. Creating Tables...';
:r 02_CreateTables.sql

-- Step 3: Seed Data
PRINT '';
PRINT '3. Seeding Initial Data...';
:r 03_SeedData.sql

PRINT '';
PRINT '=== DMT Database Setup Completed ===';
GO
