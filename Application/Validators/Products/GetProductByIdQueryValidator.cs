using Application.Mediatr.Queries.Products;
using FluentValidation;

namespace Application.Validators.Products;

public sealed class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(r => r.Id).NotEmpty().MatchesUlidFormat();
    }
}