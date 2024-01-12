using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Commands.Products;

public sealed record CreateProductCommand(Product Product) : IRequest<Product?>;