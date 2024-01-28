using Application.Mediatr.Queries.Protos;
using MediatR;

namespace GrpcService.Endpoints;

public static class ProtosEndpoints
{
    public static IEndpointRouteBuilder MapProtosEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/protos", GetProtos);
        app.MapGet("api/protos/v{version:int}/{protoName}", GetProto);

        return app;
    }

    private static async Task<IResult> GetProtos(IMediator mediator, CancellationToken cancellationToken)
    {
        var protoFiles = await mediator.Send(new GetProtosQuery(), cancellationToken);

        return Results.Ok(protoFiles);
    }

    private static async Task<IResult> GetProto(IMediator mediator, int version, string protoName,
        CancellationToken cancellationToken)
    {
        var protoFile = await mediator.Send(new GetProtoQuery(version, protoName), cancellationToken);

        if(protoFile is not null)
            return Results.Ok(protoFile);

        return Results.NotFound();
    }
}