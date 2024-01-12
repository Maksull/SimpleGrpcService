namespace Domain.Contracts;

public sealed record CursorPagedResponse<TEntity>(IEnumerable<TEntity> Items, string? Cursor, int PageSize);