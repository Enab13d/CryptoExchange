using Microsoft.Extensions.Options;
using UserService.Application.DTO.Requests;
using UserService.Application.Services.DTO;
using UserService.Infrastructure.Configuration;

namespace UserService.Application.Mappers;

public class RequestMapper(IOptions<KeycloakOptions> options) : IRequestMapper
{
    private readonly KeycloakOptions _options = options.Value;
    public KcLoginRequestDTO ToKcLoginRequest(LoginRequestDTO request)
    {
        return new KcLoginRequestDTO()
        {
            GrantType = "password",
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            Username = request.Username,
            Password = request.Password

        };
    }

    public KcLogoutRequestDTO ToKcLogoutRequest(LogoutRequestDTO request)
    {
        return new KcLogoutRequestDTO
        {
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            RefreshToken = request.RefreshToken
        };
    }

    public KcRefreshTokenRequest ToKcRefreshTokenRequest(RefreshTokenRequestDTO request)
    {
        return new KcRefreshTokenRequest()
        {
            GrantType = "refresh_token",
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            RefreshToken = request.RefreshToken
        };
    }

    public KcRegisterRequestDTO ToKcRegisterRequest(RegisterRequestDTO request)
    {
        return new KcRegisterRequestDTO()
        {
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Enabled = true,
            Credentials = [new()
            {
                Type ="password",
                Value =request.Password,
                Temporary = false
            }],
            RealmRoles = ["user"]
        };
    }
}