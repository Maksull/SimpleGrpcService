using Application.Validators;
using FluentValidation;
using SimpleGrpcProject;

namespace GrpcService.Validators.Products;

public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Description).NotEmpty();
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
    }
}
