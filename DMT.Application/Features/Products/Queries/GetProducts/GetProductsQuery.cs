using DMT.Application.Dtos;
using MediatR;

namespace DMT.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 10
) : IRequest<GetProductsResponse>;

public record GetProductsResponse(
    IEnumerable<ProductDto> Products,
    int TotalCount,
    int Page,
    int PageSize
);