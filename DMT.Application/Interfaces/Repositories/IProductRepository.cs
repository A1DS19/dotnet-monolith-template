using DMT.Domain.Entities;

namespace DMT.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
    Task<Product> UpdateAsync(Product product);
    Task DeleteAsync(int id);
}
