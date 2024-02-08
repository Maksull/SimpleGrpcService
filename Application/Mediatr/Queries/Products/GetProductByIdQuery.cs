using Application.Mediatr.Generics;
using Domain.Entities;

namespace Application.Mediatr.Queries.Products;

public sealed record GetProductByIdQuery(string Id) : ICachedQuery<Product?>
{
    public string Key => $"product-by-id-{Id}";

    public TimeSpan? Expiration => null;
}