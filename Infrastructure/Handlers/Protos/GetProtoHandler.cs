using Application.Mediatr.Queries.Protos;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Handlers.Protos;

public sealed class GetProtoHandler : IRequestHandler<GetProtoQuery, string?>
{
    private readonly IWebHostEnvironment _webHost;

    public GetProtoHandler(IWebHostEnvironment webHost)
    {
        _webHost = webHost;
    }

    public async Task<string?> Handle(GetProtoQuery request, CancellationToken cancellationToken)
    {
        string baseDirectory = _webHost.ContentRootPath;

        var filePath = $"{baseDirectory}/protos/v{request.Version}/{request.ProtoName}";
        var exist = File.Exists(filePath);

        if (exist)
            return await File.ReadAllTextAsync(filePath, cancellationToken);

        return null;
    }
}