using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Queries.Categories;

public sealed record GetCategoryByIdQuery(string Id) : IRequest<Category?>;