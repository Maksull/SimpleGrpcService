using MediatR;

namespace Application.Mediatr.Queries.Protos;

public sealed record GetProtosQuery : IRequest<Dictionary<string, IEnumerable<string?>>>;