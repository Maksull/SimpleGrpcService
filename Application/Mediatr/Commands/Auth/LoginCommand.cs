using MediatR;

namespace Application.Mediatr.Commands.Auth;

public sealed record LoginCommand(string Login, string Password) : IRequest<string?>;