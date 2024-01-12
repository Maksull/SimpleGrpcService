using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Commands.Categories;

public sealed record CreateCategoryCommand(Category Category) : IRequest<Category>;