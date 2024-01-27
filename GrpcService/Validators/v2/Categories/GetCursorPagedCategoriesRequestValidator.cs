using FluentValidation;
using SimpleGrpcProject.v2;

namespace GrpcService.Validators.v2.Categories;

public sealed class GetCursorPagedCategoriesRequestValidator : AbstractValidator<GetCursorPagedCategoriesRequest>
{
    public GetCursorPagedCategoriesRequestValidator()
    {
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}