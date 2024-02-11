using Domain.Entities;
using Domain.Entities.Product;
using MediatR;

namespace Application.Mediatr.Commands.Products;

public sealed record AddProductToDeletedProductsCommand(Product Product) : IRequest<DeletedProduct?>;