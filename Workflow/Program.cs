using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using SharedContracts;
using Workflow.Infrastructure.Configuration;
using Workflow.Infrastructure.Policies;
using Workflow.IntegrationEvents.Handlers;
using Workflow.Services;
using Workflow.Workflows.CryptoPayoutWorkflow;
using Workflow.Workflows.CryptoPayoutWorkflow.Steps;
using Workflow.Workflows.FiatOnRampWorkflow;
using Workflow.Workflows.FiatOnRampWorkflow.Steps;
using WorkflowCore.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
var mongoConn = builder.Configuration["Mongo:ConnectionString"] ?? throw new InvalidOperationException("Missing Mongo connection string");
var mongoDatabaseName = builder.Configuration["Mongo:DatabaseName"] ?? throw new InvalidOperationException("Missing Mongo db database");

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(mongoConn));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));

// Configure the HTTP request pipeline.
builder.Services.AddWorkflow(x => x.UseMongoDB(mongoConn, mongoDatabaseName));

builder.Services.AddTransient<IWorkflowService, WorkflowService>();
builder.Services.AddTransient<IWorkflow<FiatOnRampMessage>, FiatOnRampWorkflow>();
builder.Services.AddTransient<SendToLiqPayProviderStep>();
builder.Services.AddTransient<SendToSignalRProviderStep>();

builder.Services.AddTransient<IWorkflow<CryptoPayoutMessage>, CryptoPayoutWorkflow>();
builder.Services.AddTransient<SendToBlockchainProviderStep>();

// Add MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LiqpayResponseReceivedEventHandler>();
    x.AddConsumer<FormDataReceivedEventHandler>();
    x.AddConsumer<WebsocketConnectionEstablishedEventHandler>();
    x.AddConsumer<PaymentDataRequestedEventHandler>();

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
JwtOptions jwtOptions = builder.Configuration.GetRequiredSection(nameof(JwtOptions))
.Get<JwtOptions>() ?? throw new InvalidOperationException("JWT options not defined");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = jwtOptions.Authority;
    options.MapInboundClaims = false;
    options.RequireHttpsMetadata = false; //dev only
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "role",
        ValidIssuer = jwtOptions.ValidIssuer,
        ValidateIssuer = false,
        ValidAudience = jwtOptions.ValidAudience,
        ValidateAudience = true
    };
});
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(WorkflowAuthorizationPolicy.AdminPolicy, policy => policy.RequireRole("admin"))
    .AddPolicy(WorkflowAuthorizationPolicy.UserPolicy, policy =>
     policy.RequireRole("exchange-currency"));

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("api/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Get workflow registry to register your workflows
var registry = app.Services.GetRequiredService<IWorkflowRegistry>();

// Register your workflow
var fiatOnRampWorkflow = app.Services.GetRequiredService<IWorkflow<FiatOnRampMessage>>();
var cryptoPayoutWorkflow = app.Services.GetRequiredService<IWorkflow<CryptoPayoutMessage>>();
registry.RegisterWorkflow(fiatOnRampWorkflow);
registry.RegisterWorkflow(cryptoPayoutWorkflow);

var host = app.Services.GetRequiredService<IWorkflowHost>();
host.Start();

app.Run();
