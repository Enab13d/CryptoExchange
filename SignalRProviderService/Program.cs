
using MassTransit;
using SignalRProviderService.Api.Hubs;
using SignalRProviderService.Commands;
using SignalRProviderService.IntegrationEvents.Handlers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;


services.AddOpenApi();
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

app.MapHub<PaymentHub>("api/hub/payment");

app.Run();

