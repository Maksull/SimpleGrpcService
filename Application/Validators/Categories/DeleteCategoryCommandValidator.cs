using Application.Mediatr.Commands.Categories;
using FluentValidation;

namespace Application.Validators.Categories;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty().MatchesUlidFormat();
    }
}