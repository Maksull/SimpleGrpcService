using Application.Validators;
using FluentValidation;
using SimpleGrpcProject.v2;

namespace GrpcService.Validators.v2.Products;

public sealed class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().MatchesUlidFormat();
    }
}