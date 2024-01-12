using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Commands.Categories;

public sealed record UpdateCategoryCommand(Category Category) : IRequest<Category?>;