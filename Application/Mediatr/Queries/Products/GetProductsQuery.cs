using Application.Mediatr.Generics;
using Domain.Entities;
using Domain.Entities.Product;

namespace Application.Mediatr.Queries.Products;

public sealed record GetProductsQuery : ICachedQuery<IEnumerable<Product>>
{
    public string Key => $"products";

    public TimeSpan? Expiration => null;
}