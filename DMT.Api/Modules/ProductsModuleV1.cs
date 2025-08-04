public class ProductsModuleV1 : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/products")
                        .WithApiVersionSet()
                        .MapToApiVersion(1.0)
                        .WithTags("Products");

        group.MapGet("/", () => "Hello from products")
            .WithName("Get products")
            .WithSummary("Gets a list of paginated products");
    }
}
