using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Queries.Products;

public sealed record GetPagedProductsQuery(
    int Page,
    int PageSize,
    string? SortOrder) : IRequest<PagedResponse<Product>>;