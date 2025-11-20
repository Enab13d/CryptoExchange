using BlockchainProviderService.Application.Services;
using BlockchainProviderService.Infrastracture.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
IServiceCollection services = builder.Services;
services.Configure<BlockchainProviderOptions>(builder.Configuration.GetSection(nameof(BlockchainProviderOptions)));
services.AddTransient<EthereumProviderService>();
services.AddTransient<IBlockchainServiceFactory, BlockchainServiceFactory>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var x = app.Services.GetRequiredService<EthereumProviderService>();
await x.CheckBalanceAsync();
app.Run();


