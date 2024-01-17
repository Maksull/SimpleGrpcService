using Application.Mediatr.Commands.Products;
using FluentValidation;

namespace Application.Validators.Products;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty().MatchesUlidFormat();
    }
}