using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for User entity
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        #region Entity Configuration
        builder.ToTable("users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Auth0UserId)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(u => u.FirstName)
               .HasMaxLength(100);

        builder.Property(u => u.LastName)
               .HasMaxLength(100);

        builder.Property(u => u.DisplayName)
               .HasMaxLength(200);

        builder.Property(u => u.ProfilePicture)
               .HasMaxLength(500);

        builder.Property(u => u.PhoneNumber)
               .HasMaxLength(20);

        builder.Property(u => u.DeactivatedByUserId)
               .HasMaxLength(255);

        builder.HasIndex(u => u.Auth0UserId)
               .IsUnique();

        builder.HasIndex(u => u.Email)
               .IsUnique();

        // Self-referencing relationships for audit trails
        builder.HasMany(u => u.SentNotifications)
               .WithOne(n => n.Sender)
               .HasForeignKey(n => n.SenderUserId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.ReceivedNotifications)
               .WithOne(n => n.Recipient)
               .HasForeignKey(n => n.RecipientUserId)
               .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
