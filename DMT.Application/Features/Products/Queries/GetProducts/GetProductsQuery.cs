using DMT.Application.Common.CQRS;
using DMT.Application.Common.Pagination;
using DMT.Application.Dtos;

namespace DMT.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 10
) : IQuery<PaginatedResponse<ProductDto>>;
