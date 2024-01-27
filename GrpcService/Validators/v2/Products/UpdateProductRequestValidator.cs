using Application.Validators;
using FluentValidation;
using SimpleGrpcProject.v2;

namespace GrpcService.Validators.v2.Products;

public sealed class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().MatchesUlidFormat();
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Description).NotEmpty();
        RuleFor(r => r.CategoryId).NotEmpty().MatchesUlidFormat();
    }
}
