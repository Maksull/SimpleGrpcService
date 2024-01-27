using FluentValidation;
using SimpleGrpcProject.v1;

namespace GrpcService.Validators.v1.Categories;

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(r => r.CategoryName).NotEmpty();
    }
}
