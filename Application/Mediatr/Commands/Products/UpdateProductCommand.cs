using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Commands.Products;

public sealed record UpdateProductCommand(Product Product) : IRequest<Product?>;