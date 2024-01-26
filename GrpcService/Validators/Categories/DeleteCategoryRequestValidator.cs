using Application.Validators;
using FluentValidation;
using SimpleGrpcProject;

namespace GrpcService.Validators.Categories;

public sealed class DeleteCategoryRequestValidator : AbstractValidator<DeleteCategoryRequest>
{
    public DeleteCategoryRequestValidator()
    {
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
    }
}