using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for Tag entity
/// </summary>
public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        #region Entity Configuration
        builder.ToTable("tags");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(t => t.Description)
               .HasMaxLength(200);
        builder.Property(t => t.Color)
               .HasMaxLength(7); // #FFFFFF format

        builder.Property(t => t.Icon)
               .HasMaxLength(10);

        builder.Property(t => t.CreatedByUserId)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(t => t.UpdatedByUserId)
               .HasMaxLength(255);

        builder.Property(t => t.DeactivatedByUserId)
               .HasMaxLength(255);

        builder.HasIndex(t => t.Name)
               .IsUnique();

        builder.HasIndex(t => t.IsActive);
        builder.HasIndex(t => t.UsageCount);
        #endregion

        #region Data Seed
        builder.HasData(
            new Tag
            {
                Id = 1,
                Name = "happyhour",
                Description = "Happy hour drink specials",
                Color = "#FF6B35",
                Icon = "🍻",
                IsActive = true,
                UsageCount = 0,
                CreatedAt = NodaConstants.UnixEpoch,
                CreatedByUserId = "system-seed"
            },
            new Tag
            {
                Id = 2,
                Name = "livemusic",
                Description = "Live music performances",
                Color = "#4ECDC4",
                Icon = "🎵",
                IsActive = true,
                UsageCount = 0,
                CreatedAt = NodaConstants.UnixEpoch,
                CreatedByUserId = "system-seed"
            },
            new Tag
            {
                Id = 3,
                Name = "trivia",
                Description = "Trivia nights and quiz events",
                Color = "#45B7D1",
                Icon = "🧠",
                IsActive = true,
                UsageCount = 0,
                CreatedAt = NodaConstants.UnixEpoch,
                CreatedByUserId = "system-seed"
            },
            new Tag
            {
                Id = 4,
                Name = "karaoke",
                Description = "Karaoke nights",
                Color = "#FFA07A",
                Icon = "🎤",
                IsActive = true,
                UsageCount = 0,
                CreatedAt = NodaConstants.UnixEpoch,
                CreatedByUserId = "system-seed"
            },
            new Tag
            {
                Id = 5,
                Name = "foodspecial",
                Description = "Food discounts and meal deals",
                Color = "#98D8C8",
                Icon = "🍽️",
                IsActive = true,
                UsageCount = 0,
                CreatedAt = NodaConstants.UnixEpoch,
                CreatedByUserId = "system-seed"
            }
        );
        #endregion
    }
}
