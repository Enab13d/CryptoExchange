using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTO.Requests;
using UserService.Application.DTO.Responses;
using UserService.Application.Mappers;
using UserService.Application.Services;

namespace UserService.Application.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AuthController(IKeycloakClient keycloak, IRequestMapper requestMapper, IResponseMapper responseMapper, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IKeycloakClient _keycloak = keycloak;

    private readonly ILogger<AuthController> _logger = logger;
    private readonly IRequestMapper _requestMapper = requestMapper;
    private readonly IResponseMapper _responseMapper = responseMapper;
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request from Angular SPA {user}", request.Username);
        KCLoginRequestDTO kCLoginRequest = _requestMapper.ToKCLoginRequest(request);
        KCLoginResponseDTO response = await _keycloak.Login(kCLoginRequest, cancellationToken);

        return Ok(_responseMapper.ToTokenDTO(response));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request from Angular SPA {user}", request.Username);
        KCRegisterRequestDTO kCRegisterRequest = _requestMapper.ToKCRegisterRequest(request);
        try
        {

            await _keycloak.Register(kCRegisterRequest, cancellationToken);
            return Ok("Register success");
        }
        catch (Exception ex)
        {
            _logger.LogTrace("{ex}", ex);
            return BadRequest();
        }
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request from Angular SPA with refresh token {refresh}", request.RefreshToken);
        KCRefreshTokenRequest kCRefreshTokenRequest = _requestMapper.ToKCRefreshTokenRequest(request);
        KCRefreshTokenResponseDTO kCRefreshTokenResponse = await _keycloak.RefreshToken(kCRefreshTokenRequest, cancellationToken);
        TokenDTO tokenDTO = _responseMapper.ToTokenDTO(kCRefreshTokenResponse);
        _logger.LogInformation("Refresh token sucess. New access {access}", tokenDTO.Access);
        return Ok(tokenDTO);

    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequestDTO request, CancellationToken cancellationToken)
    {
        KCLogoutRequestDTO kCLogoutRequest = _requestMapper.ToKCLogoutRequest(request);
        await _keycloak.Logout(kCLogoutRequest, cancellationToken);
        _logger.LogInformation("Logout sucess");
        return NoContent();

    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserInfo(CancellationToken cancellationToken)
    {
        string? authHeader = HttpContext.Request.Headers.Authorization.ToString();
        _logger.LogInformation("Auth header {header}", authHeader);
        string token = authHeader.Split(" ", 2)[1];
        _logger.LogInformation("Token {token}", token);
        UserInfoResponseDTO userInfo = await _keycloak.GetUserInfoAsync(token, cancellationToken);
        return Ok(userInfo);


    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO request, CancellationToken cancellationToken)
    {
        try
        {
            await _keycloak.SendResetPasswordEmailAsync(request.Username, cancellationToken);
            _logger.LogInformation("Reset password success, please check email");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error {msg}", ex.Message);
            return BadRequest("Invalid username");
        }

    }
}