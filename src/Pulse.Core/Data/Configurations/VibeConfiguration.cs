using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for Vibe entity
/// </summary>
public class VibeConfiguration : IEntityTypeConfiguration<Vibe>
{
    public void Configure(EntityTypeBuilder<Vibe> builder)
    {
        #region Entity Configuration
        builder.ToTable("vibes");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(v => v.Description)
               .HasMaxLength(200);

        builder.Property(v => v.Color)
               .HasMaxLength(7); // #FFFFFF format

        builder.Property(v => v.Icon)
               .HasMaxLength(10);

        builder.HasIndex(v => v.Name)
               .IsUnique();

        builder.HasIndex(v => v.IsActive);
        builder.HasIndex(v => v.UsageCount);
        #endregion

        #region Data Seed
        builder.HasData(
            new Vibe
            {
                Id = 1,
                Name = "busy",
                Description = "High energy, crowded atmosphere",
                Color = "#FF6B35",
                Icon = "🔥",
                IsSystemVibe = true,
                IsActive = true,
                UsageCount = 0
            },
            new Vibe
            {
                Id = 2,
                Name = "chill",
                Description = "Relaxed, laid-back atmosphere",
                Color = "#4ECDC4",
                Icon = "😌",
                IsSystemVibe = true,
                IsActive = true,
                UsageCount = 0
            },
            new Vibe
            {
                Id = 3,
                Name = "lively",
                Description = "Fun and energetic atmosphere",
                Color = "#45B7D1",
                Icon = "🎉",
                IsSystemVibe = true,
                IsActive = true,
                UsageCount = 0
            },
            new Vibe
            {
                Id = 4,
                Name = "quiet",
                Description = "Peaceful and calm atmosphere",
                Color = "#A8E6CF",
                Icon = "🤫",
                IsSystemVibe = true,
                IsActive = true,
                UsageCount = 0
            },
            new Vibe
            {
                Id = 5,
                Name = "romantic",
                Description = "Perfect for couples and intimate conversations",
                Color = "#FFB6C1",
                Icon = "💕",
                IsSystemVibe = true,
                IsActive = true,
                UsageCount = 0
            }
        );
        #endregion
    }
}
