using MediatR;
using LiqPayProviderService.Domain;
using LiqPayProviderService.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace LiqPayProviderService.Infrastructure.Context;


public class PaymentDbContext(DbContextOptions<PaymentDbContext> options, IMediator mediator) : DbContext(options), IUnitOfWork
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    public DbSet<Payment> Payments { get; set; }



    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<PaymentInfo>().ToCollection("payments");

    //     modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
    // }
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // await _mediator.Publish(this, cancellationToken);
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}