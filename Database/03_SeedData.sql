USE [DMT];
GO

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
SELECT 
    [ID],
    [Name],
    [Price],
    [CreatedAt],
    [UpdatedAt]
FROM [dbo].[Products]
ORDER BY [ID];
GO
