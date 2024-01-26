using FluentValidation;
using SimpleGrpcProject;

namespace GrpcService.Validators.Products;

public sealed class GetCursorPagedProductsRequestValidator : AbstractValidator<GetCursorPagedProductsRequest>
{
    public GetCursorPagedProductsRequestValidator()
    {
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}