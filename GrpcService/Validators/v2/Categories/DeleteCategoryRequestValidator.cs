using Application.Validators;
using FluentValidation;
using SimpleGrpcProject.v2;

namespace GrpcService.Validators.v2.Categories;

public sealed class DeleteCategoryRequestValidator : AbstractValidator<DeleteCategoryRequest>
{
    public DeleteCategoryRequestValidator()
    {
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
    }
}