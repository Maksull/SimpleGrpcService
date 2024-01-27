using Application.Validators;
using FluentValidation;
using SimpleGrpcProject.v1;

namespace GrpcService.Validators.v1.Categories;

public sealed class GetCategoryByIdRequestValidator : AbstractValidator<GetCategoryRequest>
{
    public GetCategoryByIdRequestValidator()
    {
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
    }
}