using DMT.Application.Common.Caching;
using DMT.Application.Common.CQRS;
using DMT.Application.Common.Pagination;
using DMT.Application.Dtos;

namespace DMT.Application.Features.Products.Queries.GetProducts;

[Cache(durationMinutes: 15, keyPattern: "products:page:{Page}:size:{PageSize}")]
public record GetProductsQuery(
    int Page = 1,
    int PageSize = 10
) : IQuery<PaginatedResponse<ProductDto>>;
