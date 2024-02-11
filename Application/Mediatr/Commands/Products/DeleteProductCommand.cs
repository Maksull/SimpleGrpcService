using Domain.Entities;
using Domain.Entities.Product;
using MediatR;

namespace Application.Mediatr.Commands.Products;

public record DeleteProductCommand(string Id) : IRequest<Product?>;