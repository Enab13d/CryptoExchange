using LiqPayProviderService.Commands;
using LiqPayProviderService.Domain;
using LiqPayProviderService.Infrastructure.Configuration;
using LiqPayProviderService.Infrastructure.Context;
using LiqPayProviderService.Infrastructure.Repositories;
using LiqPayProviderService.IntegrationEvents.Handlers;
using LiqPayProviderService.Services;
using MassTransit;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<LiqPayOptions>(builder.Configuration.GetSection(nameof(LiqPayOptions)));
builder.Services.Configure<WebhookOptions>(builder.Configuration.GetSection(nameof(WebhookOptions)));
builder.Services.Configure<AzureCosmosOptions>(builder.Configuration.GetSection(nameof(AzureCosmosOptions)));
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILiqpayService, LiqPayService>();
builder.Services.AddDbContext<PaymentDbContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ProcessDepositCommand>());


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DepositRequestedEventHandler>();

    x.SetKebabCaseEndpointNameFormatter();
    if (builder.Environment.IsDevelopment())
    {
        x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        cfg.ReceiveEndpoint("deposit-requested", e =>
        {
            e.ConfigureConsumer<DepositRequestedEventHandler>(context);
        }
        );

        // Configure endpoints here if needed
        cfg.ConfigureEndpoints(context);
    });
    }
    else
    {
        x.UsingAzureServiceBus((context, cfg) =>
        {
            cfg.Host(builder.Configuration["AzureServiceBus:ConnectionString"]);

            cfg.ReceiveEndpoint("deposit-requested", e =>
            {
                e.ConfigureConsumer<DepositRequestedEventHandler>(context);
            });

            cfg.ConfigureEndpoints(context);

        });
    }

});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "exchange:";
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
