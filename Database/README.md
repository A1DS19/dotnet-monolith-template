# Database Setup

This folder contains SQL scripts to set up the DMT database and tables.

## Files

- `01_CreateDatabase.sql` - Creates the DMT database
- `02_CreateTables.sql` - Creates all required tables and indexes
- `03_SeedData.sql` - Inserts sample data for testing
- `RunAll.sql` - Executes all scripts in order

## Usage

### Option 1: Run Individual Scripts
Execute the scripts in order using SQL Server Management Studio or Azure Data Studio:

1. `01_CreateDatabase.sql`
2. `02_CreateTables.sql` 
3. `03_SeedData.sql`

### Option 2: Run All at Once
Execute `RunAll.sql` to run all scripts automatically.

### Option 3: Docker SQL Server
Connect to your Docker SQL Server instance:

```bash
# Connect using sqlcmd
docker exec -it dmt-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -C

# Then run:
:r /path/to/RunAll.sql
```

## Table Structure

### Products
- `ID` (int, Primary Key, Identity)
- `Name` (nvarchar(255), Not Null)
- `Price` (real, Not Null)  
- `CreatedAt` (datetime2, Default: GETUTCDATE())
- `UpdatedAt` (datetime2, Default: GETUTCDATE(), Auto-updated via trigger)

## Features

- ✅ Auto-incrementing ID
- ✅ Audit timestamps (CreatedAt, UpdatedAt)
- ✅ Update trigger for timestamp management
- ✅ Performance index on Name column
- ✅ Sample data for testing
- ✅ Idempotent scripts (safe to run multiple times)
