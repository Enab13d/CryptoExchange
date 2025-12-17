
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SignalRProviderService.Api.Hubs;
using SignalRProviderService.Commands;
using SignalRProviderService.Infrastructure.Configuration;
using SignalRProviderService.Infrastructure.Policies;
using SignalRProviderService.IntegrationEvents.Handlers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;


services.AddOpenApi();
JwtOptions jwtOptions = builder.Configuration.GetRequiredSection(nameof(JwtOptions))
.Get<JwtOptions>() ?? throw new InvalidOperationException("JWT options not defined");

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = jwtOptions.Authority;
    options.MapInboundClaims = false;
    options.RequireHttpsMetadata = false; //dev only
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "role",
        ValidIssuer = jwtOptions.ValidIssuer,
        ValidateIssuer = true,
        ValidAudience = jwtOptions.ValidAudience,
        ValidateAudience = true,
        ValidateLifetime = true
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
            path.StartsWithSegments("/api/hub/payment"))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(SignalRProviderAuthorizationPolicy.AdminPolicy, policy => policy.RequireRole("admin"))
    .AddPolicy(SignalRProviderAuthorizationPolicy.UserPolicy, policy =>
     policy.RequireRole("exchange-currency"));
services.AddSignalR();
services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<JoinHubGroupCommand>());
services.AddMassTransit(busRegistrationConfigurator =>
{
    busRegistrationConfigurator.AddConsumer<PaymentDataPreparedEventHandler>();
    busRegistrationConfigurator.SetKebabCaseEndpointNameFormatter();
    busRegistrationConfigurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
         {
             h.Username(builder.Configuration["RabbitMQ:Username"] ?? throw new ArgumentException(""));
             h.Password(builder.Configuration["RabbitMQ:Password"] ?? throw new ArgumentException(""));
         });

        cfg.ConfigureEndpoints(context);
    });
}
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<PaymentHub>("api/hub/payment");

app.Run();

