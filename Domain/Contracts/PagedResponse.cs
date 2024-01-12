namespace Domain.Contracts;

public sealed record PagedResponse<TEntity>(IEnumerable<TEntity> Items, int Page, int PageSize, int TotalCount)
{
    public bool IsNextPage => Page * PageSize < TotalCount;
    public bool IsPreviousPage => Page > 1;
}