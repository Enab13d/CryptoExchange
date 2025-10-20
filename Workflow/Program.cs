using MongoDB.Driver;
using WorkflowCore.Interface;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var mongoConn = builder.Configuration["Mongo:ConnectionString"] ?? throw new InvalidOperationException("Missing Mongo connection string");
var mongoDatabaseName = builder.Configuration["Mongo:DatabaseName"] ?? throw new InvalidOperationException("Missing Mongo db database");

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(mongoConn));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));

builder.Services.AddSingleton<MongoDBInitializer>();

// Configure the HTTP request pipeline.
builder.Services.AddWorkflow(x => x.UseMongoDB(mongoConn, mongoDatabaseName));

// Add MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
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

var dbInitializer = app.Services.GetRequiredService<MongoDBInitializer>();
await dbInitializer.InitializeAsync();

// Get workflow registry to register your workflows
var registry = app.Services.GetRequiredService<IWorkflowRegistry>();

// Register your workflow
//registry.RegisterWorkflow<YourWorkflow>();

app.Run();
