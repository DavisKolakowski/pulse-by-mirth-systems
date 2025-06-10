using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;
using NodaTime;

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
        builder.Property(u => u.ProviderId)
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

        builder.HasIndex(u => u.ProviderId)
               .IsUnique();

        builder.HasIndex(u => u.Email)
               .IsUnique();        // Self-referencing relationships for audit trails
        builder.HasMany(u => u.SentNotifications)
               .WithOne(n => n.Sender)
               .HasForeignKey(n => n.SenderUserId)
               .OnDelete(DeleteBehavior.SetNull);
               
        builder.HasMany(u => u.ReceivedNotifications)
               .WithOne(n => n.Recipient)
               .HasForeignKey(n => n.RecipientUserId)
               .OnDelete(DeleteBehavior.Cascade);
        #endregion
          #region Data Seed
        builder.HasData(
            new User
            {
                Id = 1,
                ProviderId = "auth0|682e1f2e2121380bbeb56dcf",
                Email = "davis_kolakowski@mirthsystems.com",
                FirstName = "Davis",
                LastName = "Kolakowski",
                DisplayName = "davis_kolakowski",
                ProfilePicture = "https://s.gravatar.com/avatar/a23b5565a17780ebec6bc03fccd4d0f6?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fda.png",                CreatedAt = NodaConstants.UnixEpoch,
                IsActive = true
            }
        );
        #endregion
    }
}
