using MassTransit;
using MongoDB.Driver;
using SharedContracts;
using Workflow.IntegrationEvents.Handlers;
using Workflow.Services;
using Workflow.Workflows;
using Workflow.Workflows.Steps;
using WorkflowCore.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var mongoConn = builder.Configuration["Mongo:ConnectionString"] ?? throw new InvalidOperationException("Missing Mongo connection string");
var mongoDatabaseName = builder.Configuration["Mongo:DatabaseName"] ?? throw new InvalidOperationException("Missing Mongo db database");

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(mongoConn));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));

// Configure the HTTP request pipeline.
builder.Services.AddWorkflow(x => x.UseMongoDB(mongoConn, mongoDatabaseName));

builder.Services.AddTransient<IWorkflowService, WorkflowService>();
builder.Services.AddTransient<IWorkflow<FiatToCryptoMessage>, FiatToCryptoWorkflow>();
builder.Services.AddTransient<SendToLiqPayProviderStep>();
builder.Services.AddTransient<SendToSignalRProviderStep>();

// Add MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LiqpayResponseReceivedEventHandler>();
    x.AddConsumer<FormDataReceivedEventHandler>();
    x.AddConsumer<WebsocketConnectionEstablishedEventHandler>();

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

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Get workflow registry to register your workflows
var registry = app.Services.GetRequiredService<IWorkflowRegistry>();

// Register your workflow
var workflow = app.Services.GetRequiredService<IWorkflow<FiatToCryptoMessage>>();
registry.RegisterWorkflow(workflow);

var host = app.Services.GetRequiredService<IWorkflowHost>();
host.Start();

app.Run();
