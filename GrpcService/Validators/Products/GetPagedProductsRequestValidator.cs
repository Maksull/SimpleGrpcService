﻿using FluentValidation;
using SimpleGrpcProject;

namespace GrpcService.Validators.Products;

public sealed class GetPagedProductsRequestValidator : AbstractValidator<GetPagedProductsRequest>
{
    public GetPagedProductsRequestValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(50);
    }
}