using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Queries.Categories;

public sealed record GetCursorPagedCategoriesQuery(
    string Cursor,
    int PageSize,
    string? SortOrder) : IRequest<CursorPagedResponse<Category>>;