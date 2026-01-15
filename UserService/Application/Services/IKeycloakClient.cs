using UserService.Application.Services.DTO;

namespace UserService.Application.Services;

public interface IKeycloakClient
{
    public Task<KcLoginResponseDTO> Login(KcLoginRequestDTO request, CancellationToken cancellationToken = default);

    public Task<string> Register(KcRegisterRequestDTO request, CancellationToken cancellationToken = default);

    public Task<KcRefreshTokenResponseDTO> RefreshToken(KcRefreshTokenRequest request, CancellationToken cancellationToken = default);

    public Task Logout(KcLogoutRequestDTO request, CancellationToken cancellationToken = default);

    public Task<KcUserInfoResponseDTO> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken = default);

    public Task SendResetPasswordEmailAsync(string username, CancellationToken cancellationToken = default);

}