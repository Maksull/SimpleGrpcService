using Application.Mediatr.Queries.Protos;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Handlers.Protos;

public sealed class GetProtosHandler : IRequestHandler<GetProtosQuery, Dictionary<string, IEnumerable<string?>>>
{
    private readonly IWebHostEnvironment _webHost;

    public GetProtosHandler(IWebHostEnvironment webHost)
    {
        _webHost = webHost;
    }

    public Task<Dictionary<string, IEnumerable<string?>>> Handle(GetProtosQuery request, CancellationToken cancellationToken)
    {
        string baseDirectory = _webHost.ContentRootPath;

        var protoFiles = Directory.GetDirectories($"{baseDirectory}/protos")
            .Select(x => new { version = x, protos = Directory.GetFiles(x).Select(Path.GetFileName) })
            .ToDictionary(o => Path.GetRelativePath("protos",
                o.version), o => o.protos);

        return Task.FromResult(protoFiles);
    }
}