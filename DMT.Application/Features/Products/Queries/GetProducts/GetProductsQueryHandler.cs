using DMT.Application.Interfaces.Services;
using MediatR;

namespace DMT.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsResponse>
{
    private readonly IProductService _productService;

    public GetProductsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<GetProductsResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _productService.GetPagedAsync(request.Page, request.PageSize);

        return new GetProductsResponse(
            products,
            totalCount,
            request.Page,
            request.PageSize
        );
    }
}