namespace DMT.Application.Services;

public class ProductsService(IProductRepository _productRespository) : IProductService
{
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRespository.GetAllAsync();
    }
}
