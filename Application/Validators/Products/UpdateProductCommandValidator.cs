using Application.Mediatr.Commands.Products;
using FluentValidation;

namespace Application.Validators.Products;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().MatchesUlidFormat();
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Description).NotEmpty();
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
    }
}
