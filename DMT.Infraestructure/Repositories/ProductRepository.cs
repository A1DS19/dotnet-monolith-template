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
}
