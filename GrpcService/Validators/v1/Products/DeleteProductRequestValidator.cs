using Application.Validators;
using FluentValidation;
using SimpleGrpcProject.v1;

namespace GrpcService.Validators.v1.Products;

public sealed class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().MatchesUlidFormat();
    }
}