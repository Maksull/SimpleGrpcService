using Application.Mediatr.Generics;

namespace Application.Mediatr.Queries.Protos;

public sealed record GetProtosQuery : ICachedQuery<Dictionary<string, IEnumerable<string?>>>
{
    public string Key => $"protos";

    public TimeSpan? Expiration => null;
}