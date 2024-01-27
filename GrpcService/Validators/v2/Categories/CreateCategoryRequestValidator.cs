using FluentValidation;
using SimpleGrpcProject.v2;

namespace GrpcService.Validators.v2.Categories;

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(r => r.CategoryName).NotEmpty();
    }
}
