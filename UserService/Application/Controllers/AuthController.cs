using Microsoft.AspNetCore.Mvc;
using SharedContracts;
using UserService.Application.DTO.Requests;
using UserService.Application.DTO.Responses;
using UserService.Application.Extensions;
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
        await _authService.RegisterAsync(request, cancellationToken);
        return Created();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO request, CancellationToken cancellationToken)
    {
        if (request.RefreshToken is null or "")
        {
            throw new BadHttpRequestException("refresh token is null or empty string");

        }

        TokenDTO tokenDTO = await _authService.RefreshTokenAsync(request, cancellationToken);
        _logger.LogInformation("Refresh token sucess. New access {access}", tokenDTO.Access);
        return Ok(tokenDTO);

    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequestDTO request, CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync(request, cancellationToken);
        return NoContent();

    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserInfo(CancellationToken cancellationToken)
    {
        // string? authHeader = HttpContext.Request.Headers.Authorization.ToString();
        // string token = authHeader.Split(" ", 2)[1];
        // UserInfoResponseDTO userInfo = await _authService.GetUserDataAsync(token, cancellationToken);
        User userInfo = HttpContext.UserFromClaims();
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