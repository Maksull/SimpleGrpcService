using Application.Validators;
using FluentValidation;
using SimpleGrpcProject;

namespace GrpcService.Validators.Products;

public sealed class GetProductByIdRequestValidator : AbstractValidator<GetProductRequest>
{
    public GetProductByIdRequestValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().MatchesUlidFormat();
    }
}