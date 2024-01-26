using FluentValidation;
using SimpleGrpcProject;

namespace GrpcService.Validators.Categories;

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(r => r.CategoryName).NotEmpty();
    }
}
