using FluentValidation;
using SimpleGrpcProject.v2;

namespace GrpcService.Validators.v2.Products;

public sealed class GetPagedProductsRequestValidator : AbstractValidator<GetPagedProductsRequest>
{
    public GetPagedProductsRequestValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}