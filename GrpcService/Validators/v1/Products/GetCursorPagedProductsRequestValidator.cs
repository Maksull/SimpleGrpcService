using FluentValidation;
using SimpleGrpcProject.v1;

namespace GrpcService.Validators.v1.Products;

public sealed class GetCursorPagedProductsRequestValidator : AbstractValidator<GetCursorPagedProductsRequest>
{
    public GetCursorPagedProductsRequestValidator()
    {
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}