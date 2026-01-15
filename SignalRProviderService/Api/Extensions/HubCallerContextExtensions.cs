
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using SharedContracts;

namespace SignalRProviderService.Api.Extensions;


public static class HubCallerContextExtensions
{
    public static User? UserFromClaims(this HubCallerContext ctx)
    {
        ClaimsPrincipal? claims = ctx.User;
        if (claims is null) return null;
        List<string> roles = [.. claims.FindAll("role").Select(c => c.Value)];
        User user = new()
        {
            UserId = claims.FindFirst("sub")?.Value ?? "",
            Username = claims.FindFirst("preferred_username")?.Value ?? "",
            FirstName = claims.FindFirst("given_name")?.Value ?? "",
            LastName = claims.FindFirst("family_name")?.Value ?? "",
            Email = claims.FindFirst("email")?.Value ?? "",
            Roles = roles
        };
        return user;
    }
}