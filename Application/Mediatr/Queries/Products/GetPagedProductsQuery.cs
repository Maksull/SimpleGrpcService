using Application.Mediatr.Generics;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Product;

namespace Application.Mediatr.Queries.Products;

public sealed record GetPagedProductsQuery(
    int Page,
    int PageSize,
    string? SortOrder) : ICachedQuery<PagedResponse<Product>>
{
    public string Key => $"offset-paged-products-{Page}-{PageSize}";

    public TimeSpan? Expiration => null;
}