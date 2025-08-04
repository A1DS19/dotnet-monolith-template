using DMT.Application.Common.Caching;
using DMT.Application.Common.CQRS;

namespace DMT.Application.Features.Products.Commands.UpdateProduct;

[InvalidateCache("products:*", "product:{Id}")]
public record UpdateProductCommand(
    int Id,
    string Name,
    float Price
) : ICommand<UpdateProductResponse>;

public record UpdateProductResponse(
    int Id,
    string Name,
    float Price
);
