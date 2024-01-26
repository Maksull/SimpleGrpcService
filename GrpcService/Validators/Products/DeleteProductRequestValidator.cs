using Application.Validators;
using FluentValidation;
using SimpleGrpcProject;

namespace GrpcService.Validators.Products;

public sealed class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().MatchesUlidFormat();
    }
}