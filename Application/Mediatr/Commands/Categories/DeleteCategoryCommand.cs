using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Commands.Categories;

public sealed record DeleteCategoryCommand(string Id) : IRequest<Category?>;