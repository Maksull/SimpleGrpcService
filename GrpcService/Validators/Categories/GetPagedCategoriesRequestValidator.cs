using FluentValidation;
using SimpleGrpcProject;

namespace GrpcService.Validators.Categories;

public sealed class GetPagedCategoriesRequestValidator : AbstractValidator<GetPagedCategoriesRequest>
{
    public GetPagedCategoriesRequestValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}