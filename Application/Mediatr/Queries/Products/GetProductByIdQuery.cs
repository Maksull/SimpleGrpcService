using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Queries.Products;

public sealed record GetProductByIdQuery(string Id) : IRequest<Product?>;