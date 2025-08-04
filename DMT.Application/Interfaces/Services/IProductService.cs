namespace DMT.Application.Interfaces.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
}
