using Application.Serialization;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class CacheService : ICacheService
{
    private IDistributedCache _cache;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CacheService> _logger;
    private bool _isConnectionError;
    private DateTime _lastReconnectAttempt = DateTime.UtcNow;

    public CacheService(IDistributedCache cache, IConfiguration configuration, ILogger<CacheService> logger)
    {
        _cache = cache;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        try
        {
            if (IsReconnect())
            {
                await Reconnect();
            }
            else if (!_isConnectionError)
            {
                var cachedValue = await _cache.GetAsync(key, CancellationToken.None);

                if (cachedValue is null) return default;

                var value = ProtoBufSerializer.ByteArrayToClass<T>(cachedValue);

                return value;
            }

            return default;
        }
        catch (RedisConnectionException e)
        {
            _isConnectionError = true;
            _logger.LogCritical("Redis connection failed: {RedisConnectionError}", e.ToString());

            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration) where T : class
    {
        try
        {
            if (IsReconnect())
            {
                await Reconnect();
            }
            else if (!_isConnectionError)
            {
                var valueToCache = ProtoBufSerializer.ClassToByteArray(value);

                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(180))
                    .SetAbsoluteExpiration(absoluteExpiration ?? TimeSpan.FromSeconds(3600));

                await _cache.SetAsync(key, valueToCache, cacheEntryOptions, CancellationToken.None);
            }
        }
        catch (RedisConnectionException e)
        {
            _isConnectionError = true;
            _logger.LogCritical("Redis connection failed: {RedisConnectionError}", e.ToString());
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            if (IsReconnect())
            {
                await Reconnect();
            }
            else if (!_isConnectionError)
            {
                await _cache.RemoveAsync(key);
            }
        }
        catch (RedisConnectionException e)
        {
            _isConnectionError = true;
            _logger.LogCritical("Redis connection failed: {RedisConnectionError}", e.ToString());
        }
    }

    private bool IsReconnect()
    {
        const double timeBeforeReconnect = 2;

        return _isConnectionError && (DateTime.UtcNow - _lastReconnectAttempt).TotalMinutes >= timeBeforeReconnect;
    }

    private async Task Reconnect()
    {
        try
        {
            _lastReconnectAttempt = DateTime.UtcNow;
            var redisConnectionString = _configuration.GetConnectionString("RedisCache")!;
            var redis = await ConnectionMultiplexer.ConnectAsync(redisConnectionString);

            if (redis.IsConnected)
            {
                _cache = new RedisCache(new RedisCacheOptions
                {
                    Configuration = redisConnectionString,
                    InstanceName = "Redis"
                });

                _isConnectionError = false;
            }
        }
        catch (RedisConnectionException e)
        {
            _logger.LogCritical("Redis reconnect failed: {RedisReconnectError}", e.ToString());
        }
    }
}
