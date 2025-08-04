using DMT.Application.Common.Extensions;
using DMT.Application.Common.Pagination;
using DMT.Application.Dtos;
using DMT.Application.Exceptions;
using DMT.Application.Interfaces.Repositories;
using DMT.Application.Interfaces.Services;
using DMT.Domain.Entities;

namespace DMT.Application.Services;

public class ProductsService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(p => new ProductDto(p.ID, p.Name, p.Price));
    }

    public async Task<PaginatedResponse<ProductDto>> GetPagedAsync(PaginatedRequest request, CancellationToken cancellationToken = default)
    {
        var allProducts = await _productRepository.GetAllAsync();
        var productDtos = allProducts.Select(p => new ProductDto(p.ID, p.Name, p.Price));

        return productDtos.ToPaginatedResponse(request);
    }

    public async Task<Product> CreateAsync(string name, float price)
    {
        // Business validation
        if (await ExistsByNameAsync(name))
        {
            throw new AlreadyExistsException("Product", name);
        }

        // Business logic - data cleaning and rules
        var cleanName = name.Trim();
        var roundedPrice = (float)Math.Round(price, 2);

        if (string.IsNullOrWhiteSpace(cleanName))
        {
            throw new BadRequestException("Product name cannot be empty");
        }

        if (roundedPrice <= 0)
        {
            throw new BadRequestException("Product price must be greater than 0");
        }

        // Create entity
        var product = new Product
        {
            Name = cleanName,
            Price = roundedPrice
        };

        // Delegate to repository
        return await _productRepository.CreateAsync(product);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null ? new ProductDto(product.ID, product.Name, product.Price) : null;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _productRepository.ExistsAsync(id);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _productRepository.ExistsByNameAsync(name);
    }
}
