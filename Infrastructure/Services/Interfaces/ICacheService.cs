namespace Infrastructure.Services.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration, CancellationToken cancellationToken) where T : class;
    Task RemoveAsync(string key, CancellationToken cancellationToken);
}