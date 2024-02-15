using Application.Logging.ExceptionInterceptor;
using FluentValidation;
using Grpc.Core;

namespace GrpcService.Interceptors.ExceptionInterceptor;

public static class ExceptionHelpers
{
    public static RpcException Handle<T>(this Exception exception, ILogger<T> logger) =>
        exception switch
        {
            RpcException rpcException => HandleRpcException(rpcException, logger),
            ValidationException validateException => HandleValidateException(validateException, logger),
            OperationCanceledException operationCanceledException => HandleOperationCanceledException(operationCanceledException, logger),
            _ => HandleDefault(exception, logger)
        };

    private static RpcException HandleDefault<T>(Exception exception, ILogger<T> logger)
    {
        logger.LogUnhandledException(exception.ToString());
        return new RpcException(new Status(StatusCode.Internal, exception.Message));
    }

    private static RpcException HandleRpcException<T>(RpcException exception, ILogger<T> logger)
    {
        logger.LogRpcException(exception.ToString());
        return new RpcException(new Status(exception.StatusCode, exception.Status.Detail));
    }

    private static RpcException HandleValidateException<T>(ValidationException exception, ILogger<T> logger)
    {
        logger.LogValidationError(exception.ToString());
        return new RpcException(new Status(StatusCode.InvalidArgument, exception.Message));
    }

    private static RpcException HandleOperationCanceledException<T>(OperationCanceledException exception, ILogger<T> logger)
    {
        logger.LogCancelledOperation(exception.ToString());
        return new RpcException(new Status(StatusCode.Cancelled, exception.Message));
    }
}