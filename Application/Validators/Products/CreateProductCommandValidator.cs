using Application.Mediatr.Commands.Products;
using FluentValidation;

namespace Application.Validators.Products;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Description).NotEmpty();
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
    }
}
