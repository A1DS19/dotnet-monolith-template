using DMT.Application.Common.CQRS;
using DMT.Application.Interfaces.Services;

namespace DMT.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductService _productService;

    public CreateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var createdProduct = await _productService.CreateAsync(request.Name, request.Price);

        return new CreateProductResponse(
            createdProduct.ID,
            createdProduct.Name,
            createdProduct.Price
        );
    }
}
