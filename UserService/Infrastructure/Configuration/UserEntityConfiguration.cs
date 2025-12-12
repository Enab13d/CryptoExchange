using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Configuration;


public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.ToTable("users");
        builder.Property(e => e.CreatedAt)
        .HasDefaultValueSql("now()");

        builder.Property(e => e.UpdatedAt)
        .HasDefaultValueSql("now()")
        .ValueGeneratedOnAddOrUpdate();
    }
}