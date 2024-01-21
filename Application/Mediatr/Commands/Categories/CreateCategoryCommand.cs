using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Commands.Categories;

public sealed record CreateCategoryCommand(string CategoryName) : IRequest<Category>;