using LiqPayProviderService.Commands;
using LiqPayProviderService.Domain;
using LiqPayProviderService.Infrastructure.Clients.LiqpayClient;
using LiqPayProviderService.Infrastructure.Configuration;
using LiqPayProviderService.Infrastructure.Context;
using LiqPayProviderService.Infrastructure.Repositories;
using LiqPayProviderService.IntegrationEvents.Handlers;
using LiqPayProviderService.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<LiqPayClientOptions>(builder.Configuration.GetSection(nameof(LiqPayClientOptions)));
string? mongoConnectionString = builder.Configuration.GetConnectionString("MongoConnection" ?? throw new InvalidOperationException("mongoConnectionString missing"));
string? mongoDatabaseName = builder.Configuration["MongoDatabaseName"] ?? throw new InvalidOperationException("MongoDatabaseName missing");
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILiqpayService, LiqPayService>();
builder.Services.AddScoped<IWebhookService, WebhookService>();
builder.Services.AddDbContext<PaymentDbContext>((sp, options) =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    options.UseMongoDB(client, mongoDatabaseName);
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ProcessDepositCommand>());

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DepositRequestedEventHandler>();

    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        // Configure endpoints here if needed
        cfg.ConfigureEndpoints(context);
    });
});

// Add services to the container.
// implement the following
builder.Services.AddHttpClient<ILiqpayClient, LiqpayClient>(client =>
{
    client.BaseAddress = new Uri("https://www.liqpay.ua/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(name: "v1", new OpenApiInfo
    {
        Title = "Liqpay payment provider",
        Version = "v1",
        Description = "Processing payment requests"
    });

}

);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
