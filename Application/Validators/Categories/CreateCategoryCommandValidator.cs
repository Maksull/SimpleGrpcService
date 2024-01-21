using Application.Mediatr.Commands.Categories;
using FluentValidation;

namespace Application.Validators.Categories;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(r => r.CategoryName).NotEmpty();
    }
}
