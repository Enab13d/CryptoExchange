using UserService.Application.DTO.Requests;
using UserService.Application.DTO.Responses;

namespace UserService.Application.Services;

public interface IKeycloakClient
{
    public Task<KCLoginResponseDTO> Login(KCLoginRequestDTO request, CancellationToken cancellationToken = default);

    public Task Register(KCRegisterRequestDTO request, CancellationToken cancellationToken = default);

    public Task<KCRefreshTokenResponseDTO> RefreshToken(KCRefreshTokenRequest request, CancellationToken cancellationToken = default);

    public Task Logout(KCLogoutRequestDTO request, CancellationToken cancellationToken = default);

    public Task<UserInfoResponseDTO> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken = default);

    public Task SendResetPasswordEmailAsync(string username, CancellationToken cancellationToken = default);

}