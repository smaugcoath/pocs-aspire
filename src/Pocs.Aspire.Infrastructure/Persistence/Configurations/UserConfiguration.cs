using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pocs.Aspire.Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("Id");

        builder.Property(e => e.UserId)
            .HasColumnName("UserId")
            .IsRequired();
        
        // Acts as a secondary key for EF.
        builder.HasAlternateKey(e => e.UserId)
            .HasName("IX_Users_UserId");

        builder.Property(e => e.FirstName)
            .HasColumnName("FirstName")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .HasColumnName("LastName")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");
    }
}
