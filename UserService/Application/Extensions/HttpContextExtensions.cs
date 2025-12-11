using System.Security.Claims;
using SharedContracts;

namespace UserService.Application.Extensions;


public static class HttpContextExtensions
{

    public static User UserFromClaims(this HttpContext ctx)
    {
        ClaimsPrincipal claims = ctx.User;
        User user = new()
        {
            UserId = claims.FindFirst("sub")?.Value ?? "",
            Username = claims.FindFirst("preferred_username")?.Value ?? "",
            FirstName = claims.FindFirst("given_name")?.Value ?? "",
            LastName = claims.FindFirst("family_name")?.Value ?? "",
            Email = claims.FindFirst("email")?.Value ?? "",
        };
        return user;
    }



}


