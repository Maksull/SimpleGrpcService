using Application.Validators;
using FluentValidation;
using SimpleGrpcProject;

namespace GrpcService.Validators.Categories;

public sealed class GetCategoryByIdRequestValidator : AbstractValidator<GetCategoryRequest>
{
    public GetCategoryByIdRequestValidator()
    {
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
    }
}