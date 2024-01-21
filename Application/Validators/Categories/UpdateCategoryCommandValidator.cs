using Application.Mediatr.Commands.Categories;
using FluentValidation;

namespace Application.Validators.Categories;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
        RuleFor(r => r.CategoryName).NotEmpty();
    }
}
