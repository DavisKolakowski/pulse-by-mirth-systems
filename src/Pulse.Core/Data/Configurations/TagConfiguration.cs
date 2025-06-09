using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
                IsSystemTag = true,
                IsActive = true,
                UsageCount = 0
            },
            new Tag
            {
                Id = 2,
                Name = "livemusic",
                Description = "Live music performances",
                Color = "#4ECDC4",
                Icon = "🎵",
                IsSystemTag = true,
                IsActive = true,
                UsageCount = 0
            },
            new Tag
            {
                Id = 3,
                Name = "trivia",
                Description = "Trivia nights and quiz events",
                Color = "#45B7D1",
                Icon = "🧠",
                IsSystemTag = true,
                IsActive = true,
                UsageCount = 0
            },
            new Tag
            {
                Id = 4,
                Name = "karaoke",
                Description = "Karaoke nights",
                Color = "#FFA07A",
                Icon = "🎤",
                IsSystemTag = true,
                IsActive = true,
                UsageCount = 0
            },
            new Tag
            {
                Id = 5,
                Name = "foodspecial",
                Description = "Food discounts and meal deals",
                Color = "#98D8C8",
                Icon = "🍽️",
                IsSystemTag = true,
                IsActive = true,
                UsageCount = 0
            }
        );
        #endregion
    }
}
