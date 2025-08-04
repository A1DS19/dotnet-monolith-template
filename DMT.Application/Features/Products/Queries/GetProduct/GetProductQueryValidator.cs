using FluentValidation;

namespace DMT.Application.Features.Products.Queries.GetProduct;

public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
                .WithMessage("Product Id mus be greater than 0");
    }
}

