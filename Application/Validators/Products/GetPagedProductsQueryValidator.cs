using Application.Mediatr.Queries.Products;
using FluentValidation;

namespace Application.Validators.Products;

public sealed class GetPagedProductsQueryValidator : AbstractValidator<GetPagedProductsQuery>
{
    public GetPagedProductsQueryValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).GreaterThanOrEqualTo(50);
    }
}