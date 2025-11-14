using LiqPayProviderService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiqPayProviderService.Infrastructure.Configuration;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{


    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(e => e.Fiat).HasConversion<string>();

        builder.Property(e => e.Crypto).HasConversion<string>();

    }
}