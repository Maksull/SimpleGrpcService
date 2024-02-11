using Application.Mediatr.Generics;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Behaviors;

public sealed class QueryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse?> 
    where TRequest : ICachedQuery 
    where TResponse : class
{
    private readonly ICacheService _cacheService;

    public QueryCachingBehavior(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<TResponse?> Handle(TRequest request, RequestHandlerDelegate<TResponse?> next, CancellationToken cancellationToken)
    {
        var cachedValue = await _cacheService.GetAsync<TResponse>(request.Key, cancellationToken);

        if (cachedValue is not null)
            return cachedValue;

        var value = await next();

        if (value is not null)
            await _cacheService.SetAsync(request.Key, value, request.Expiration, cancellationToken);

        return value;
    }
}
