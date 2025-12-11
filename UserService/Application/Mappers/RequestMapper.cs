using Microsoft.Extensions.Options;
using UserService.Application.DTO.Requests;
using UserService.Infrastructure.Configuration;

namespace UserService.Application.Mappers;

public class RequestMapper(IOptions<KeycloakOptions> options) : IRequestMapper
{
    private readonly KeycloakOptions _options = options.Value;
    public KCLoginRequestDTO ToKCLoginRequest(LoginRequestDTO request)
    {
        return new KCLoginRequestDTO()
        {
            GrantType = "password",
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            Username = request.Username,
            Password = request.Password

        };
    }

    public KCLogoutRequestDTO ToKCLogoutRequest(LogoutRequestDTO request)
    {
        return new KCLogoutRequestDTO
        {
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            RefreshToken = request.RefreshToken
        };
    }

    public KCRefreshTokenRequest ToKCRefreshTokenRequest(RefreshTokenRequestDTO request)
    {
        return new KCRefreshTokenRequest()
        {
            GrantType = "refresh_token",
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            RefreshToken = request.RefreshToken
        };
    }

    public KCRegisterRequestDTO ToKCRegisterRequest(RegisterRequestDTO request)
    {
        return new KCRegisterRequestDTO()
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