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

        var protoFiles = Directory.GetDirectories($"{baseDirectory}/protos")
            .Select(x => new { version = x, protos = Directory.GetFiles(x).Select(Path.GetFileName) })
            .ToDictionary(o => Path.GetRelativePath("protos",
                o.version), o => o.protos);

        return Results.Ok(protoFiles);
    }

    private static async Task<IResult> GetProto(IWebHostEnvironment webHost, int version, string protoName,
        CancellationToken cancellationToken)
    {
        string baseDirectory = webHost.ContentRootPath;

        var filePath = $"{baseDirectory}/protos/v{version}/{protoName}";
        var exist = File.Exists(filePath);

        if (exist)
            return Results.Ok(await File.ReadAllTextAsync(filePath, cancellationToken));

        return Results.NotFound();
    }
}