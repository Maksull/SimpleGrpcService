using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Queries.Products;

public sealed record GetCursorPagedProductsQuery(
    string Cursor,
    int PageSize,
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder) : IRequest<CursorPagedResponse<Product>>;