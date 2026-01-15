using UserService.Application.DTO.Requests;
using UserService.Application.DTO.Responses;
using UserService.Application.Services.DTO;

namespace UserService.Application.Services;


public interface IAuthService
{
    public Task<TokenDTO> LoginAsync(LoginRequestDTO request, CancellationToken cancellationToken);

    public Task RegisterAsync(RegisterRequestDTO request, CancellationToken cancellationToken);

    public Task<TokenDTO> RefreshTokenAsync(RefreshTokenRequestDTO request, CancellationToken cancellationToken);

    public Task<KcUserInfoResponseDTO> GetUserDataAsync(string accessToken, CancellationToken cancellationToken = default);

    public Task LogoutAsync(LogoutRequestDTO request, CancellationToken cancellationToken = default);

    public Task ResetPasswordAsync(ResetPasswordRequestDTO request, CancellationToken cancellationToken = default);
}