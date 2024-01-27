namespace GrpcService.Endpoints;

public static class ProtosEndpoints
{
    public static IEndpointRouteBuilder MapProtosEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/protos", GetProtos);
        app.MapGet("api/protos/v{version:int}/{protoName}", GetProto);

        return app;
    }

    private static IResult GetProtos(IWebHostEnvironment webHost, CancellationToken cancellationToken)
    {
        string baseDirectory = webHost.ContentRootPath;

        var protoFiles = Directory.GetFiles($"{baseDirectory}/protos").Select(Path.GetFileName);

        return Results.Ok(protoFiles);
    }

    private static async Task<IResult> GetProto(IWebHostEnvironment webHost, int version, string protoName, CancellationToken cancellationToken)
    {
        string baseDirectory = webHost.ContentRootPath;
        
        var filePath = $"{baseDirectory}/protos/{protoName}";
        var exist = File.Exists(filePath);

        if (exist)
            return Results.Ok(await File.ReadAllTextAsync(filePath, cancellationToken));
        
        return Results.NotFound();
    }
}