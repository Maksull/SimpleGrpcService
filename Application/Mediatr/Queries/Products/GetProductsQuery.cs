using Application.Mediatr.Generics;
using Domain.Entities;

namespace Application.Mediatr.Queries.Products;

public sealed record GetProductsQuery : ICachedQuery<IEnumerable<Product>>
{
    public string Key => $"products";

    public TimeSpan? Expiration => null;
}