using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace LiqPayProviderService.Infrastructure.Context;


public class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Payment> Payments { get; set; }



    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<PaymentInfo>().ToCollection("payments");

    //     modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
    // }
    

}