using Application.Mediatr.Queries.Products;
using FluentValidation;

namespace Application.Validators.Products;

public sealed class GetCursorPagedProductsQueryValidator : AbstractValidator<GetCursorPagedProductsQuery>
{
    public GetCursorPagedProductsQueryValidator()
    {
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}