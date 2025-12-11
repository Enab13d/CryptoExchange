using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using UserService.Application.Mappers;
using UserService.Application.Services;
using UserService.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;
services.Configure<KeycloakOptions>(configuration.GetSection(nameof(KeycloakOptions)));
services.AddTransient<IRequestMapper, RequestMapper>();
services.AddTransient<IResponseMapper, ResponseMapper>();
services.AddHttpClient<IKeycloakClient, KeycloakClient>((sp, client) =>
{
    KeycloakOptions options = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;
    client.BaseAddress = new Uri($"{options.BaseUrl}");
});
services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;
services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
