using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Entities;
using LiqPayProviderService.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace LiqPayProviderService.Infrastructure.Context;

public class PaymentDbContext(DbContextOptions<PaymentDbContext> options, IOptions<AzureCosmosOptions> cosmosOptions) : DbContext(options), IUnitOfWork
{
    public DbSet<Payment> Payments { get; set; }

    private readonly AzureCosmosOptions _cosmosOptions = cosmosOptions.Value;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>().ToContainer("paymentsdb");
        modelBuilder.Entity<Payment>().HasNoDiscriminator().HasPartitionKey(p => p.PaymentId);


        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(_cosmosOptions.AccountEndpoint, _cosmosOptions.AccountKey, _cosmosOptions.DatabaseName);
        base.OnConfiguring(optionsBuilder);
    }



}