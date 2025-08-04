using DMT.Application.Common.Caching;
using DMT.Application.Common.CQRS;
using DMT.Application.Dtos;

namespace DMT.Application.Features.Products.Queries.GetProduct;

[Cache(durationMinutes: 30, keyPattern: "product:{Id}")]
public record GetProductQuery(int Id) : IQuery<ProductDto>;
