using BlockchainProviderService.Application.Commands;
using BlockchainProviderService.Application.Mappers;
using BlockchainProviderService.Application.Services;
using BlockchainProviderService.Infrastracture.Configuration;
using BlockchainProviderService.Infrastracture.IntegrationEvents.Handlers;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
IServiceCollection services = builder.Services;
services.Configure<BlockchainProviderOptions>(builder.Configuration.GetSection(nameof(BlockchainProviderOptions)));
services.Configure<CryptoMarketDataClientOptions>(builder.Configuration.GetSection(nameof(CryptoMarketDataClientOptions)));
services.AddScoped<ICurrencyMapper, CurrencyMapper>();
services.AddTransient<EthereumProviderService>();
services.AddTransient<IBlockchainServiceFactory, BlockchainServiceFactory>();
services.AddHttpClient<ICryptoMarketDataClient, CryptoMarketDataClient>((sp, client) =>
{
    CryptoMarketDataClientOptions options = sp.GetRequiredService<IOptions<CryptoMarketDataClientOptions>>().Value;
    client.BaseAddress = new Uri(options.ProviderAPIURL);
    client.DefaultRequestHeaders.TryAddWithoutValidation("X-CMC_PRO_API_KEY", options.PrivateKey);
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ProcessCryptoPayoutCommand>());

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CryptoPayoutRequestedEventHandler>();

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
    cfg.ReceiveEndpoint("crypto-payout-requested", e =>
    {
        e.ConfigureConsumer<CryptoPayoutRequestedEventHandler>(context);
    });
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
                e.ConfigureConsumer<CryptoPayoutRequestedEventHandler>(context);
            });

            cfg.ConfigureEndpoints(context);

        });
    }



});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();


