using Microsoft.Extensions.Logging;

namespace Application.Logging.Caching;

public static partial class RedisOutputCacheStoreLogs
{
    [LoggerMessage(EventId = LogEvents.RedisConnectionFailedId, EventName = LogEvents.RedisConnectionFailedName,
       Level = LogLevel.Critical,
       Message = "Redis connection failed: {RedisConnectionError}",
       SkipEnabledCheck = true)]
    public static partial void LogRedisConnectionFailed(this ILogger logger, string redisConnectionError);
}
