using FluentValidation;
using SimpleGrpcProject;

namespace GrpcService.Validators.Categories;

public sealed class GetCursorPagedCategoriesRequestValidator : AbstractValidator<GetCursorPagedCategoriesRequest>
{
    public GetCursorPagedCategoriesRequestValidator()
    {
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}