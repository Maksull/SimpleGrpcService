using Application.Mediatr.Generics;

namespace Application.Mediatr.Queries.Protos;

public sealed record GetProtoQuery(int Version, string ProtoName) : ICachedQuery<string?>
{
    public string Key => $"proto-{Version}-{ProtoName}";

    public TimeSpan? Expiration => null;
}