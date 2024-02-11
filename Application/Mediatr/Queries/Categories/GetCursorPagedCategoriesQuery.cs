using Application.Mediatr.Generics;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Category;

namespace Application.Mediatr.Queries.Categories;

public sealed record GetCursorPagedCategoriesQuery(
    string Cursor,
    int PageSize,
    string? SortOrder) : ICachedQuery<CursorPagedResponse<Category>>
{
    public string Key => $"cursor-paged-categories-{Cursor}-{PageSize}";

    public TimeSpan? Expiration => null;
}