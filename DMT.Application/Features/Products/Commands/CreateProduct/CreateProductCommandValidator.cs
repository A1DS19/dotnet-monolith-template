using FluentValidation;

namespace DMT.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Product name is required")
            .MaximumLength(255)
                .WithMessage("Product name must not exceed 255 characters")
            .MinimumLength(2)
                .WithMessage("Product name must be at least 2 characters long");

        RuleFor(x => x.Price)
            .GreaterThan(0)
                .WithMessage("Product price must be greater than 0")
            .LessThanOrEqualTo(999999.99f)
                .WithMessage("Product price must not exceed 999,999.99");
    }
}
