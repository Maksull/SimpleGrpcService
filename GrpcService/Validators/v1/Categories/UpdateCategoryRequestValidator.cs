using Application.Validators;
using FluentValidation;
using SimpleGrpcProject.v1;

namespace GrpcService.Validators.v1.Categories;

public sealed class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
        RuleFor(r => r.Name).NotEmpty();
    }
}
