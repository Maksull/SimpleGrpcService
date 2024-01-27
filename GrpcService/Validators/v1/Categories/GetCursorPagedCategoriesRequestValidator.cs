using FluentValidation;
using SimpleGrpcProject.v1;

namespace GrpcService.Validators.v1.Categories;

public sealed class GetCursorPagedCategoriesRequestValidator : AbstractValidator<GetCursorPagedCategoriesRequest>
{
    public GetCursorPagedCategoriesRequestValidator()
    {
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}