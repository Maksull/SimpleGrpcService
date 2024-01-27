using FluentValidation;
using SimpleGrpcProject.v2;

namespace GrpcService.Validators.v2.Products;

public sealed class GetCursorPagedProductsRequestValidator : AbstractValidator<GetCursorPagedProductsRequest>
{
    public GetCursorPagedProductsRequestValidator()
    {
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}