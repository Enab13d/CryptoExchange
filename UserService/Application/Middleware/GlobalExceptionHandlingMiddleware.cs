using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Api.Middleware;


public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {

        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException br)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Bad request",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Detail = br.Message
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (KeyNotFoundException knf)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Resource not found",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Detail = knf.Message
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (AuthenticationException authenticationEx)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ProblemDetails problemDetails = new()
            {
                Status = context.Response.StatusCode,
                Title = "Unauthorized",
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                Detail = authenticationEx.Message
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (UnauthorizedAccessException unauthorized)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            ProblemDetails problemDetails = new()
            {
                Status = context.Response.StatusCode,
                Title = "Forbidden",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                Detail = unauthorized.Message
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occured");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            string traceId = context.TraceIdentifier;
            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "An error occured",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Detail = $"See logs for details. TraceId = {traceId}",


            };
            problemDetails.Extensions["traceId"] = traceId;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

}