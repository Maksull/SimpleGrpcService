using Domain.Entities;
using MediatR;

namespace Application.Mediatr.Queries.Products;

public sealed record GetProductsQuery : IRequest<IEnumerable<Product>>;