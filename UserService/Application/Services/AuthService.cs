using UserService.Application.DTO.Requests;
using UserService.Application.DTO.Responses;
using UserService.Application.Mappers;
using UserService.Application.Services.DTO;
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
    public async Task<KcUserInfoResponseDTO> GetUserDataAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        KcUserInfoResponseDTO userInfo = await _keycloak.GetUserInfoAsync(accessToken, cancellationToken);
        return userInfo;
    }

    public async Task<TokenDTO> LoginAsync(LoginRequestDTO request, CancellationToken cancellationToken)
    {
        _ = await _userRepository.GetByEmailAsync(request.Username, cancellationToken)
        ?? throw new BadHttpRequestException("Invalid credentials");

        KcLoginRequestDTO kCLoginRequest = _requestMapper.ToKcLoginRequest(request);
        var response = await _keycloak.Login(kCLoginRequest, cancellationToken);
        return _responseMapper.ToTokenDTO(response);
    }

    public async Task LogoutAsync(LogoutRequestDTO request, CancellationToken cancellationToken = default)
    {
        KcLogoutRequestDTO kCLogoutRequest = _requestMapper.ToKcLogoutRequest(request);
        await _keycloak.Logout(kCLogoutRequest, cancellationToken);
    }

    public async Task<TokenDTO> RefreshTokenAsync(RefreshTokenRequestDTO request, CancellationToken cancellationToken)
    {
        KcRefreshTokenRequest kCRefreshTokenRequest = _requestMapper.ToKcRefreshTokenRequest(request);
        KcRefreshTokenResponseDTO kCRefreshTokenResponse = await _keycloak.RefreshToken(kCRefreshTokenRequest, cancellationToken);
        return _responseMapper.ToTokenDTO(kCRefreshTokenResponse);
    }

    public async Task RegisterAsync(RegisterRequestDTO request, CancellationToken cancellationToken)
    {
        var userFromDb = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (userFromDb is not null) throw new BadHttpRequestException("User with this email already exist");
        KcRegisterRequestDTO kCRegisterRequest = _requestMapper.ToKcRegisterRequest(request);
        try
        {

            string userId = await _keycloak.Register(kCRegisterRequest, cancellationToken);
            User user = ModelToEnity.UserFromRegisterRequest(request);
            user.Id = userId;

            await _userRepository.InsertAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
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