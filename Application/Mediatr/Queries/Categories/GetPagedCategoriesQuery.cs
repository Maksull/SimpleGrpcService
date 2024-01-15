using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Queries.Categories;

public sealed record GetPagedCategoriesQuery(
    string? SortOrder,
    int Page,
    int PageSize) : IRequest<PagedResponse<Category>>;