using DMT.Application.Dtos;
using DMT.Domain.Entities;

namespace DMT.Application.Interfaces.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<(IEnumerable<ProductDto> Products, int TotalCount)> GetPagedAsync(int page, int pageSize);
    Task<Product> CreateAsync(string name, float price);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
}
