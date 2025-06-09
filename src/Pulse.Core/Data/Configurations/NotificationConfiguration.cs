using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for Notification entity
/// </summary>
public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        #region Entity Configuration
        builder.ToTable("notifications");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Title)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(n => n.Message)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(n => n.ActionUrl)
               .HasMaxLength(500);

        builder.Property(n => n.ImageUrl)
               .HasMaxLength(500);

        builder.HasOne(n => n.Recipient)
               .WithMany(u => u.ReceivedNotifications)
               .HasForeignKey(n => n.RecipientUserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.Sender)
               .WithMany(u => u.SentNotifications)
               .HasForeignKey(n => n.SenderUserId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(n => n.RelatedVenue)
               .WithMany(v => v.RelatedNotifications)
               .HasForeignKey(n => n.RelatedVenueId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(n => n.RelatedSpecial)
               .WithMany(s => s.RelatedNotifications)
               .HasForeignKey(n => n.RelatedSpecialId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(n => n.RelatedTag)
               .WithMany()
               .HasForeignKey(n => n.RelatedTagId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(n => n.RecipientUserId);
        builder.HasIndex(n => n.Type);
        builder.HasIndex(n => n.CreatedAt);
        builder.HasIndex(n => new { n.RecipientUserId, n.IsRead });
        #endregion
    }
}
