using DMT.Application.Common.Pagination;
using DMT.Application.Dtos;
using DMT.Application.Features.Products.Commands.CreateProduct;
using DMT.Application.Features.Products.Queries.GetProducts;
using MediatR;

public class ProductsModuleV1 : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/products")
                        .WithApiVersionSet()
                        .MapToApiVersion(1.0)
                        .WithTags("Products");

        group.MapGet("/", async (IMediator mediator, int page = 1, int pageSize = 10) =>
        {
            var query = new GetProductsQuery(page, pageSize);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetProducts")
        .WithSummary("Gets a list of paginated products")
        .Produces<PaginatedResponse<ProductDto>>()
        .ProducesValidationProblem();

        group.MapPost("/", async (IMediator mediator, CreateProductCommand command) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/v1/products/{result.Id}", result);
        })
        .WithName("CreateProduct")
        .WithSummary("Creates a new product")
        .Produces<CreateProductResponse>(201)
        .ProducesValidationProblem();
    }
}
