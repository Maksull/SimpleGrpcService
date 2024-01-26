using Application.Mediatr.Queries.Categories;
using FluentValidation;

namespace Application.Validators.Categories;

public sealed class GetPagedCategoriesQueryValidator : AbstractValidator<GetPagedCategoriesQuery>
{
    public GetPagedCategoriesQueryValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}