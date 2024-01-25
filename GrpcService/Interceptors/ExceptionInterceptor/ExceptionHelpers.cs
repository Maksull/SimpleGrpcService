using FluentValidation;
using Grpc.Core;

namespace GrpcService.Interceptors.ExceptionInterceptor;

public static class ExceptionHelpers
{
    public static RpcException Handle<T>(this Exception exception,
        ServerCallContext context, ILogger<T> logger) =>
        exception switch
        {
            RpcException rpcException => HandleRpcException(rpcException, context,
                logger),
            ValidationException validateException => HandleValidateException(validateException, context,
                logger),
            OperationCanceledException operationCanceledException => HandleOperationCanceledException(operationCanceledException, context,
                logger),
            _ => HandleDefault(exception, context, logger)
        };

    private static RpcException HandleDefault<T>(Exception exception, ServerCallContext context,
        ILogger<T> logger)
    {
        logger.LogError("An Exception occured, {Exception}", exception.ToString());
        return new RpcException(new Status(StatusCode.Internal, exception.Message));
    }

    private static RpcException HandleRpcException<T>(RpcException exception, ServerCallContext context,
        ILogger<T> logger)
    {
        logger.LogInformation("RpcException occured, {RpcException}", exception.ToString());
        return new RpcException(new Status(exception.StatusCode, exception.Status.Detail));
    }

    private static RpcException HandleValidateException<T>(ValidationException exception, ServerCallContext context,
        ILogger<T> logger)
    {
        logger.LogError("Validation error occured, {ValidationException}", exception.ToString());
        return new RpcException(new Status(StatusCode.InvalidArgument, exception.Message));
    }

    private static RpcException HandleOperationCanceledException<T>(OperationCanceledException exception, ServerCallContext context,
        ILogger<T> logger)
    {
        logger.LogInformation("Operation was cancelled {OperationCanceledException}", exception.ToString());
        return new RpcException(new Status(StatusCode.Cancelled, exception.Message));
    }
}