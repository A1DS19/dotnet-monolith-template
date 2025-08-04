using DMT.Application.Dtos;
using DMT.Application.Exceptions;
using DMT.Application.Interfaces.Repositories;
using MediatR;

namespace DMT.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetProductsResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync();

        var productDtos = products.Select(p => new ProductDto(p.Name, p.Price));

        var pagedProducts = productDtos
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        return new GetProductsResponse(
            pagedProducts,
            products.Count(),
            request.Page,
            request.PageSize
        );
    }
}
