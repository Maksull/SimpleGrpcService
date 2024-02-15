using Microsoft.Extensions.Logging;

namespace Application.Logging.ExceptionInterceptor;

public static partial class ExceptionInterceptorLogs
{
    [LoggerMessage(EventId = LogEvents.UnhandledExceptionId, EventName = LogEvents.UnhandledExceptionName,
        Level = LogLevel.Error,
        Message = "An unhandled exception occured, {UnhandledException}",
        SkipEnabledCheck = true)]
    public static partial void LogUnhandledException(this ILogger logger, string unhandledException);
    
    [LoggerMessage(EventId = LogEvents.RpcExceptionId, EventName = LogEvents.RpcExceptionName,
        Level = LogLevel.Information,
        Message = "RpcException occured, {RpcException}",
        SkipEnabledCheck = true)]
    public static partial void LogRpcException(this ILogger logger, string rpcException);
    
    [LoggerMessage(EventId = LogEvents.ValidationErrorId, EventName = LogEvents.ValidationErrorName,
        Level = LogLevel.Error,
        Message = "Validation error occured, {ValidationException}",
        SkipEnabledCheck = true)]
    public static partial void LogValidationError(this ILogger logger, string validationError);
    
    [LoggerMessage(EventId = LogEvents.CancelledOperationId, EventName = LogEvents.CancelledOperationName,
        Level = LogLevel.Information,
        Message = "Operation was cancelled {OperationCanceledException}",
        SkipEnabledCheck = true)]
    public static partial void LogCancelledOperation(this ILogger logger, string operationCanceledException);
}