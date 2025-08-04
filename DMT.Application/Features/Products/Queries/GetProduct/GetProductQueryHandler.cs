using DMT.Application.Common.CQRS;
using DMT.Application.Dtos;
using DMT.Application.Interfaces.Services;

namespace DMT.Application.Features.Products.Queries.GetProduct;

public class GetProductQueryHandler : IQueryHandler<GetProductQuery, ProductDto>
{
    private readonly IProductService _productService;

    public GetProductQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var products = await _productService.GetPagedAsync(
            new Application.Common.Pagination.PaginatedRequest(1, 1000),
            cancellationToken);

        return products.Data.FirstOrDefault(p => p.Id == request.Id)
            ?? throw new Application.Exceptions.NotFoundException($"Product with ID {request.Id} not found");
    }
}
