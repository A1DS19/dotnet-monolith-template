using DMT.Application.Dtos;
using MediatR;

namespace DMT.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    float Price
) : IRequest<CreateProductResponse>;

public record CreateProductResponse(
    int Id,
    string Name,
    float Price
);
