using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Api.Middleware;
using UserService.Application.Services;
using UserService.Domain.SeedWork;
using UserService.Infrastructure.Configuration;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;
services.AddDbContext<UsersDbContext>();
services.Configure<KeycloakOptions>(configuration.GetSection(nameof(KeycloakOptions)));
services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddHttpClient<IKeycloakClient, KeycloakClient>((sp, client) =>
{
    KeycloakOptions options = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;
    client.BaseAddress = new Uri($"{options.BaseUrl}");
});
services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;
services.AddOpenApi();
JwtOptions jwtOptions = builder.Configuration.GetRequiredSection(nameof(JwtOptions))
.Get<JwtOptions>() ?? throw new InvalidOperationException("JWT options not defined");
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
db.Database.Migrate();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.Run();
