using MediatR;

namespace Application.Mediatr.Generics;

public interface ICachedQuery<TResponse> : IRequest<TResponse>, ICachedQuery;

public interface ICachedQuery
{
    string Key { get; }
    TimeSpan? Expiration { get; }
}
