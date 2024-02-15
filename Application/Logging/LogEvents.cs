namespace Application.Logging;

public static class LogEvents
{
    public const int UnhandledExceptionId = 1001;
    public const string UnhandledExceptionName = "UnhandledException";

    public const int RpcExceptionId = 1002;
    public const string RpcExceptionName = "RpcException";

    public const int ValidationErrorId = 1003;
    public const string ValidationErrorName = "ValidationError";
    
    public const int CancelledOperationId = 1004;
    public const string CancelledOperationName = "CancelledOperation";
    
    public const int IncomingId = 1005;
    public const string IncomingName = "Incoming";
    
    public const int RedisConnectionFailedId = 1006;
    public const string RedisConnectionFailedName = "RedisConnectionFailed";
}