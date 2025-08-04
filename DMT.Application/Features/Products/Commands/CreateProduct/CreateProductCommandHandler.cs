using DMT.Application.Exceptions;
using DMT.Application.Interfaces.Repositories;
using DMT.Domain.Entities;
using MediatR;

namespace DMT.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        var createdProduct = await _productRepository.CreateAsync(product);

        return new CreateProductResponse(
            createdProduct.ID,
            createdProduct.Name,
            createdProduct.Price
        );
    }
}
