using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Queries.Categories;

public sealed record GetPagedCategoriesQuery(
    int Page,
    int PageSize,
    string? SortOrder) : IRequest<PagedResponse<Category>>;