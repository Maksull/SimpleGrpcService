using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Mediatr.Commands.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Handlers.Auth;

public sealed class LoginHandler : IRequestHandler<LoginCommand, string?>
{
    private readonly IConfiguration _configuration;

    public LoginHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (request is not { Login: "Admin", Password: "Admin" }) return Task.FromResult<string?>(null);
        
        string token = CreateToken();

        return Task.FromResult(token)!;

    }
    
    private string CreateToken()
    {
        List<Claim> claims =
        [
            new(ClaimTypes.Role, "Admin")
        ];

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:SecurityKey").Value!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(_configuration.GetSection("JwtSettings:ExpiresInMinutes").Value!)),
            signingCredentials: credentials
        );

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}