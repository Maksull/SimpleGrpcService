using Domain.Entities;
using Domain.Entities.Product;
using MediatR;

namespace Application.Mediatr.Commands.Products;

public sealed record UpdateProductCommand(string ProductId, string Name, string Description, string CategoryId) : IRequest<Product?>;