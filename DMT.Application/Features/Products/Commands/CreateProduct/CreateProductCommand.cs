using DMT.Application.Common.Caching;
using DMT.Application.Common.CQRS;

namespace DMT.Application.Features.Products.Commands.CreateProduct;

[InvalidateCache("products:*")]
public record CreateProductCommand(
    string Name,
    float Price
) : ICommand<CreateProductResponse>;

public record CreateProductResponse(
    int Id,
    string Name,
    float Price
);
