using Application.Mediatr.Queries.Categories;
using FluentValidation;

namespace Application.Validators.Categories;

public sealed class GetCursorPagedCategoriesQueryValidator : AbstractValidator<GetCursorPagedCategoriesQuery>
{
    public GetCursorPagedCategoriesQueryValidator()
    {
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}