using MediatR;
using MongoDB.Driver;

namespace Application.Mediatr.Commands.Products;

public sealed record DeleteProductsPermanentlyCommand : IRequest<DeleteResult>;