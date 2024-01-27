using Application.Validators;
using FluentValidation;
using SimpleGrpcProject.v1;

namespace GrpcService.Validators.v1.Products;

public sealed class GetProductByIdRequestValidator : AbstractValidator<GetProductRequest>
{
    public GetProductByIdRequestValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().MatchesUlidFormat();
    }
}