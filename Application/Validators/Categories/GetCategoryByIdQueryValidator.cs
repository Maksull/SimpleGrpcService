using Application.Mediatr.Queries.Categories;
using FluentValidation;

namespace Application.Validators.Categories;

public sealed class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
    }
}