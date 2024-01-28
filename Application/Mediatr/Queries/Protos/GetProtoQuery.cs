using MediatR;

namespace Application.Mediatr.Queries.Protos;

public sealed record GetProtoQuery(int Version, string ProtoName) : IRequest<string?>;