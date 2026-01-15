using UserService.Application.DTO.Requests;
using UserService.Application.DTO.Responses;
using UserService.Application.Services.DTO;
using UserService.Domain.Entities;
using UserService.Infrastructure.Configuration;

namespace UserService.Application.Extensions;

public static class DtoExtenstions
{

    public static KcLoginRequestDTO ToKcLoginRequest(this LoginRequestDTO request, KeycloakOptions options)
    => new()
    {
        GrantType = "password",
        ClientId = options.ClientId,
        ClientSecret = options.ClientSecret,
        Username = request.Username,
        Password = request.Password

    };

    public static KcLogoutRequestDTO ToKcLogoutRequest(this LogoutRequestDTO request, KeycloakOptions options)
    => new()
    {
        ClientId = options.ClientId,
        ClientSecret = options.ClientSecret,
        RefreshToken = request.RefreshToken
    };

    public static KcRefreshTokenRequest ToKcRefreshTokenRequest(this RefreshTokenRequestDTO request, KeycloakOptions options)
    => new()
    {
        GrantType = "refresh_token",
        ClientId = options.ClientId,
        ClientSecret = options.ClientSecret,
        RefreshToken = request.RefreshToken
    };

    public static KcRegisterRequestDTO ToKcRegisterRequest(this RegisterRequestDTO request)
    => new()
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

    public static TokenDTO ToTokenDTO(this KcLoginResponseDTO response)
    {
        return new TokenDTO()
        {
            Access = response.AcessToken,
            Refresh = response.RefreshToken
        };
    }
    public static TokenDTO ToTokenDTO(this KcRefreshTokenResponseDTO response)
    {
        return new TokenDTO()
        {
            Access = response.AcessToken,
            Refresh = response.RefreshToken
        };
    }
    public static User ToUser(this RegisterRequestDTO dto)
    => new()
    {
        Username = dto.Username,
        Email = dto.Email,
        FirstName = dto.FirstName,
        LastName = dto.LastName
    };
}