using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Queries.Products;

public sealed record GetPagedProductsQuery(
    string? SortOrder,
    int Page,
    int PageSize) : IRequest<PagedResponse<Product>>;