using Microsoft.Extensions.Logging;

namespace Application.Logging.GrpcServices;

public static partial class GrpcServices
{
    [LoggerMessage(EventId = LogEvents.IncomingId, EventName = LogEvents.IncomingName,
        Level = LogLevel.Information,
        Message = "Incoming {IncomingData}",
        SkipEnabledCheck = true)]
    public static partial void LogIncoming(this ILogger logger, string incomingData);
}