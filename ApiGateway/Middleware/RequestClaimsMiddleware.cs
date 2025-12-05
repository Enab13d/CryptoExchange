using System.Security.Claims;

namespace ApiGateway.Middleware;

public class RequestClaimsMiddleware(RequestDelegate next, ILogger<RequestClaimsMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<RequestClaimsMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext ctx)
    {
        if (ctx.User?.Identity?.IsAuthenticated == true)
        {
            var userId = ctx.User.FindFirst("sub")?.Value ?? "";
            var username = ctx.User.FindFirst("preferred_username")?.Value ?? "unknown";
            var roles = string.Join(",", ctx.User.FindAll("role").Select(c => c.Value));

            foreach (Claim claim in ctx.User.Claims)
            {
                _logger.LogInformation(
               "Claim:\n  Issuer: {Issuer}\n  Type:   {Type}\n  Value:  {Value}",
               claim.Issuer,
               claim.Type,
               claim.Value
           );

            }

            _logger.LogInformation("Authenticated → {User} ({Id}) | Roles: {Roles}", username, userId, roles);
        }
        else
        {
            _logger.LogWarning("Unauthenticated request");
        }

        await _next(ctx);
    }
}