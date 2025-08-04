using DMT.Application.Common.CQRS;
using DMT.Application.Common.Extensions;
using DMT.Application.Common.Pagination;
using DMT.Application.Dtos;
using DMT.Application.Interfaces.Services;

namespace DMT.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, PaginatedResponse<ProductDto>>
{
    private readonly IProductService _productService;

    public GetProductsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<PaginatedResponse<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var paginatedRequest = new PaginatedRequest(request.Page, request.PageSize);
        return await _productService.GetPagedAsync(paginatedRequest, cancellationToken);
    }
}
