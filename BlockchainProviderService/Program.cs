using BlockchainProviderService.Application.Mappers;
using BlockchainProviderService.Application.Services;
using BlockchainProviderService.Infrastracture.Configuration;
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();


