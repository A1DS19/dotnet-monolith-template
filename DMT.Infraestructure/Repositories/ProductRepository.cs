namespace DMT.Infrastructure.Repositories;

using Dapper;
using DMT.Application.Interfaces.Repositories;
using DMT.Domain.Entities;
using DMT.Infrastructure.Data;

public class ProductRepository : BaseRepository, IProductRepository
{
    public ProductRepository(IDbConnectionFactory connectionFactory)
          : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await ExecuteAsync(async connection =>
        {
            const string sql = "SELECT ID, Name, Price FROM Products";
            return await connection.QueryAsync<Product>(sql);
        });
    }

    public async Task<Product> CreateAsync(Product product)
    {
        return await ExecuteAsync(async connection =>
        {
            const string sql = @"
                INSERT INTO Products (Name, Price) 
                OUTPUT INSERTED.ID, INSERTED.Name, INSERTED.Price
                VALUES (@Name, @Price)";

            return await connection.QuerySingleAsync<Product>(sql, product);
        });
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await ExecuteAsync(async connection =>
        {
            const string sql = "SELECT ID, Name, Price FROM Products WHERE ID = @id";
            return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { id });
        });
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await ExecuteAsync(async connection =>
        {
            const string sql = "SELECT COUNT(1) FROM Products WHERE ID = @id";
            var count = await connection.QuerySingleAsync<int>(sql, new { id });
            return count > 0;
        });
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await ExecuteAsync(async connection =>
        {
            const string sql = "SELECT COUNT(1) FROM Products WHERE LOWER(Name) = LOWER(@name)";
            var count = await connection.QuerySingleAsync<int>(sql, new { name });
            return count > 0;
        });
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        return await ExecuteAsync(async connection =>
        {
            const string sql = @"
                UPDATE Products 
                SET Name = @Name, Price = @Price 
                OUTPUT INSERTED.ID, INSERTED.Name, INSERTED.Price
                WHERE ID = @ID";

            return await connection.QuerySingleAsync<Product>(sql, product);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await ExecuteAsync(async connection =>
        {
            const string sql = "DELETE FROM Products WHERE ID = @id";
            await connection.ExecuteAsync(sql, new { id });
        });
    }
}
