using UserService.Application.DTO.Requests;
using UserService.Application.Services.DTO;

namespace UserService.Application.Mappers;


public interface IRequestMapper
{
    public KcLoginRequestDTO ToKcLoginRequest(LoginRequestDTO request);

    public KcRegisterRequestDTO ToKcRegisterRequest(RegisterRequestDTO request);

    public KcRefreshTokenRequest ToKcRefreshTokenRequest(RefreshTokenRequestDTO request);

    public KcLogoutRequestDTO ToKcLogoutRequest(LogoutRequestDTO request);

};