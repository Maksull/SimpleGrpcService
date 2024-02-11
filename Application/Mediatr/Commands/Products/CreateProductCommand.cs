using Domain.Entities;
using Domain.Entities.Product;
using MediatR;

namespace Application.Mediatr.Commands.Products;

public sealed record CreateProductCommand(string Name, string Description, string CategoryId) : IRequest<Product?>;