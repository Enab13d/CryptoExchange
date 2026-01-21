using ApiGateway.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"configuration.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSignalR();
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins(allowedOrigins!)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        ;
    });
});
JwtOptions jwtOptions = builder.Configuration.GetRequiredSection(nameof(JwtOptions))
.Get<JwtOptions>() ?? throw new InvalidOperationException("JWT options not defined");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = jwtOptions.Authority;
    options.MapInboundClaims = false;
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment(); //dev only
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "role",
        ValidIssuer = jwtOptions.ValidIssuer,
        ValidateIssuer = true,
        ValidAudience = jwtOptions.ValidAudience,
        ValidateAudience = true,
        ValidateLifetime = true
    };
});


builder.Services.AddAuthorization();
var app = builder.Build();
app.UseWebSockets();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
await app.UseOcelot();
await app.RunAsync();
