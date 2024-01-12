using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Commands.Products;

public record DeleteProductCommand(string Id) : IRequest<Product?>;