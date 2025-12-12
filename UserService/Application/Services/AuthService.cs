using UserService.Application.DTO.Requests;
using UserService.Application.DTO.Responses;
using UserService.Application.Mappers;
using UserService.Domain.Entities;
using UserService.Domain.SeedWork;
using UserService.Infrastructure.Repositories;

namespace UserService.Application.Services;

public class AuthService(IKeycloakClient keycloak, IRequestMapper requestMapper, IResponseMapper responseMapper, ILogger<AuthService> logger, IUserRepository userRepository, IUnitOfWork unitOfWork) : IAuthService
{
    private readonly IKeycloakClient _keycloak = keycloak;
    private readonly IRequestMapper _requestMapper = requestMapper;
    private readonly IResponseMapper _responseMapper = responseMapper;
    private readonly IUserRepository _userRepository = userRepository;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<AuthService> _logger = logger;
    public async Task<UserInfoResponseDTO> GetUserDataAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        UserInfoResponseDTO userInfo = await _keycloak.GetUserInfoAsync(accessToken, cancellationToken);
        return userInfo;
    }

    public async Task<TokenDTO> LoginAsync(LoginRequestDTO request, CancellationToken cancellationToken)
    {
        KCLoginRequestDTO kCLoginRequest = _requestMapper.ToKCLoginRequest(request);
        KCLoginResponseDTO response = await _keycloak.Login(kCLoginRequest, cancellationToken);
        return _responseMapper.ToTokenDTO(response);
    }

    public async Task LogoutAsync(LogoutRequestDTO request, CancellationToken cancellationToken = default)
    {
        KCLogoutRequestDTO kCLogoutRequest = _requestMapper.ToKCLogoutRequest(request);
        try
        {
            await _keycloak.Logout(kCLogoutRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error processing logout request {err}", ex.Message);
            throw;
        }
    }

    public async Task<TokenDTO> RefreshTokenAsync(RefreshTokenRequestDTO request, CancellationToken cancellationToken)
    {
        KCRefreshTokenRequest kCRefreshTokenRequest = _requestMapper.ToKCRefreshTokenRequest(request);
        KCRefreshTokenResponseDTO kCRefreshTokenResponse = await _keycloak.RefreshToken(kCRefreshTokenRequest, cancellationToken);
        return _responseMapper.ToTokenDTO(kCRefreshTokenResponse);
    }

    public async Task RegisterAsync(RegisterRequestDTO request, CancellationToken cancellationToken)
    {
        KCRegisterRequestDTO kCRegisterRequest = _requestMapper.ToKCRegisterRequest(request);
        try
        {

            string userId = await _keycloak.Register(kCRegisterRequest, cancellationToken);
            User user = ModelToEnity.UserFromRegisterRequest(request);
            user.Id = userId;
            await _userRepository.InsertAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("{ex}", ex);
            throw;
        }
    }

    public async Task ResetPasswordAsync(ResetPasswordRequestDTO request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _keycloak.SendResetPasswordEmailAsync(request.Username, cancellationToken);
            _logger.LogInformation("Reset password success, please check email");
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error {msg}", ex.Message);
            throw;
        }
    }
}