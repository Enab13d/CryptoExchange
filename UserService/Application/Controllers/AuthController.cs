using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTO.Requests;
using UserService.Application.DTO.Responses;
using UserService.Application.Services;

namespace UserService.Application.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AuthController(ILogger<AuthController> logger, IAuthService authService) : ControllerBase
{

    private readonly ILogger<AuthController> _logger = logger;

    private readonly IAuthService _authService = authService;
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request from Angular SPA {user}", request.Username);
        TokenDTO token = await _authService.LoginAsync(request, cancellationToken);

        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request from Angular SPA {user}", request.Username);
        try
        {

            await _authService.RegisterAsync(request, cancellationToken);
            return Ok("Register success");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.StackTrace);
        }
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request from Angular SPA with refresh token {refresh}", request.RefreshToken);

        TokenDTO tokenDTO = await _authService.RefreshTokenAsync(request, cancellationToken);
        _logger.LogInformation("Refresh token sucess. New access {access}", tokenDTO.Access);
        return Ok(tokenDTO);

    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequestDTO request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.LogoutAsync(request, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.StackTrace);
        }

    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserInfo(CancellationToken cancellationToken)
    {
        string? authHeader = HttpContext.Request.Headers.Authorization.ToString();
        string token = authHeader.Split(" ", 2)[1];
        UserInfoResponseDTO userInfo = await _authService.GetUserDataAsync(token, cancellationToken);
        return Ok(userInfo);


    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.ResetPasswordAsync(request, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.StackTrace);
        }

    }
}