namespace DMT.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> CreateAsync(Product product);
}
