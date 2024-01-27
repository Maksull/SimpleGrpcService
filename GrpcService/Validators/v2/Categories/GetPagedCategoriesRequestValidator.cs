using FluentValidation;
using SimpleGrpcProject.v2;

namespace GrpcService.Validators.v2.Categories;

public sealed class GetPagedCategoriesRequestValidator : AbstractValidator<GetPagedCategoriesRequest>
{
    public GetPagedCategoriesRequestValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}