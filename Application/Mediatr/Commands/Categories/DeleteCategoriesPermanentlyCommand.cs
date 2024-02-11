using MediatR;
using MongoDB.Driver;

namespace Application.Mediatr.Commands.Categories;

public sealed record DeleteCategoriesPermanentlyCommand : IRequest<DeleteResult>;