using ApiGateway.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("configuration.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSignalR();
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        ; ;
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = "http://keycloak:8080/realms/ce-realm";
    options.MapInboundClaims = false;
    options.RequireHttpsMetadata = false; //dev only
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "role",
        ValidIssuer = "http://localhost:18080/realms/ce-realm",
        ValidateIssuer = false,
        ValidAudience = "ce-client",
        ValidateAudience = true
    };
});


builder.Services.AddAuthorization();
var app = builder.Build();
app.UseWebSockets();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseMiddleware<RequestClaimsMiddleware>();
app.UseAuthorization();
await app.UseOcelot();
await app.RunAsync();
