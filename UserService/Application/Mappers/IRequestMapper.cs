using UserService.Application.DTO.Requests;

namespace UserService.Application.Mappers;


public interface IRequestMapper
{
    public KCLoginRequestDTO ToKCLoginRequest(LoginRequestDTO request);

    public KCRegisterRequestDTO ToKCRegisterRequest(RegisterRequestDTO request);

    public KCRefreshTokenRequest ToKCRefreshTokenRequest(RefreshTokenRequestDTO request);

    public KCLogoutRequestDTO ToKCLogoutRequest(LogoutRequestDTO request);

};