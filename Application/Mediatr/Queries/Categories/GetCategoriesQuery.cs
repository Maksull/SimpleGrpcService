using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Queries.Categories;

public sealed record GetCategoriesQuery : IRequest<IEnumerable<Category>>;