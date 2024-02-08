using Application.Mediatr.Generics;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Mediatr.Queries.Products;

public sealed record GetCursorPagedProductsQuery(
    string Cursor,
    int PageSize,
    string? SortOrder) : ICachedQuery<CursorPagedResponse<Product>>
{
    public string Key => $"cursor-paged-products-{Cursor}-{PageSize}";

    public TimeSpan? Expiration => null;
}