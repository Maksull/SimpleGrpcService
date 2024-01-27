using Application.Validators;
using FluentValidation;
using SimpleGrpcProject.v2;

namespace GrpcService.Validators.v2.Products;

public sealed class GetProductByIdRequestValidator : AbstractValidator<GetProductRequest>
{
    public GetProductByIdRequestValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().MatchesUlidFormat();
    }
}