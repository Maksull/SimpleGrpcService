using Application.Mediatr.Commands.Auth;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SimpleGrpcProject.v1;

namespace GrpcService.Services.v1;

public sealed class GrpcAuthService : AuthServiceProto.AuthServiceProtoBase
{
    private readonly IMediator _mediator;

    public GrpcAuthService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var jwt = (await _mediator.Send(new LoginCommand(request.Login, request.Password), context.CancellationToken) ?? "");

        return new()
        {
            Jwt = jwt
        };
    }

    [Authorize]
    public override Task<TestAuthResponse> TestAuth(TestAuthRequest request, ServerCallContext context)
    {
        TestAuthResponse t = new()
        {
            Response = request.Message + " RESPONSE MESSAGE"
        };

        return Task.FromResult(t);
    }
}