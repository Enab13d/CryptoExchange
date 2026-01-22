
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SignalRProviderService.Api.Commands.Handlers;
using SignalRProviderService.Api.Hubs;
using SignalRProviderService.Infrastructure.Configuration;
using SignalRProviderService.Infrastructure.Policies;


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
services.AddMassTransit(busRegistrationConfigurator =>
{
    busRegistrationConfigurator.AddConsumer<PaymentDataPreparedEventHandler>();
    busRegistrationConfigurator.SetKebabCaseEndpointNameFormatter();
    if (builder.Environment.IsDevelopment())
    {
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

    else
    {
        busRegistrationConfigurator.UsingAzureServiceBus((context, cfg) =>
        {
            cfg.Host(builder.Configuration["AzureServiceBus:ConnectionString"]);

            cfg.ReceiveEndpoint("deposit-requested", e =>
            {
                e.ConfigureConsumer<PaymentDataPreparedEventHandler>(context);
            });

            cfg.ConfigureEndpoints(context);

        });
    }
}
);


// services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration["Redis:ConnectionString"];
//     options.InstanceName = "exchange:";
// });
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

