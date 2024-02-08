using Application.Mediatr.Generics;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Mediatr.Queries.Categories;

public sealed record GetPagedCategoriesQuery(
    int Page,
    int PageSize,
    string? SortOrder) : ICachedQuery<PagedResponse<Category>>
{
    public string Key => $"offset-paged-categories-{Page}-{PageSize}";

    public TimeSpan? Expiration => null;
}