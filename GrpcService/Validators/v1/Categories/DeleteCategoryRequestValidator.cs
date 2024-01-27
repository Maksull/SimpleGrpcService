using Application.Validators;
using FluentValidation;
using SimpleGrpcProject.v1;

namespace GrpcService.Validators.v1.Categories;

public sealed class DeleteCategoryRequestValidator : AbstractValidator<DeleteCategoryRequest>
{
    public DeleteCategoryRequestValidator()
    {
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
    }
}