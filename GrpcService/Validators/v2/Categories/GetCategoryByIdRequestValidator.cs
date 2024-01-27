using Application.Validators;
using FluentValidation;
using SimpleGrpcProject.v2;

namespace GrpcService.Validators.v2.Categories;

public sealed class GetCategoryByIdRequestValidator : AbstractValidator<GetCategoryRequest>
{
    public GetCategoryByIdRequestValidator()
    {
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
    }
}