using FluentValidation;

namespace DMT.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
                .WithMessage("Page number must be greater than 0")
            .LessThanOrEqualTo(1000)
                .WithMessage("Page number must not exceed 1000");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
                .WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100)
                .WithMessage("Page size must not exceed 100 items");
    }
}
