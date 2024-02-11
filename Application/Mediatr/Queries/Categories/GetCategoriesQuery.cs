using Application.Mediatr.Generics;
using Domain.Entities;
using Domain.Entities.Category;

namespace Application.Mediatr.Queries.Categories;

public sealed record GetCategoriesQuery : ICachedQuery<IEnumerable<Category>>
{
    public string Key => $"categories";

    public TimeSpan? Expiration => null;
}