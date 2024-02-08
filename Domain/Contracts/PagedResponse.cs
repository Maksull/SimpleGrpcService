using ProtoBuf;

namespace Domain.Contracts;

[ProtoContract]
public sealed record PagedResponse<TEntity>
{
    [ProtoMember(1)]
    public IEnumerable<TEntity> Items { get; init; } = Enumerable.Empty<TEntity>().ToList();

    [ProtoMember(2)]
    public int Page { get; init; }

    [ProtoMember(3)]
    public int PageSize { get; init; }

    [ProtoMember(4)]
    public int TotalCount { get; init; }

    public PagedResponse()
    {
        // Parameterless constructor required by Protobuf
    }

    public PagedResponse(IEnumerable<TEntity> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public bool IsNextPage => Page * PageSize < TotalCount;
    public bool IsPreviousPage => Page > 1;
}