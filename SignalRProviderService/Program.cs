
using MassTransit;
using SignalRProviderService.Api.Hubs;
using SignalRProviderService.IntegrationEvents.Handlers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;



services.AddOpenApi();
services.AddSignalR();
services.AddMassTransit(busRegistrationConfigurator =>
{
    busRegistrationConfigurator.AddConsumer<PaymentDataPreparedEventHandler>();
    busRegistrationConfigurator.SetKebabCaseEndpointNameFormatter();
    busRegistrationConfigurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
         {
             h.Username(builder.Configuration["RabbitMQ:Username"]);
             h.Password(builder.Configuration["RabbitMQ:Password"]);
         });

        // Configure endpoints here if needed
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

app.MapHub<PaymentHub>("/payment-data");

app.Run();

