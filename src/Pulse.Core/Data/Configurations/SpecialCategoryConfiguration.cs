using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for SpecialCategory entity
/// </summary>
public class SpecialCategoryConfiguration : IEntityTypeConfiguration<SpecialCategory>
{
    public void Configure(EntityTypeBuilder<SpecialCategory> builder)
    {
        #region Entity Configuration
        builder.ToTable("special_categories");
        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(sc => sc.Description)
               .HasMaxLength(200);

        builder.Property(sc => sc.Icon)
               .HasMaxLength(10);
        builder.HasIndex(sc => sc.Name)
               .IsUnique();
        #endregion
        
        #region Data Seed
        builder.HasData(
            new SpecialCategory
            {
                Id = 1,
                Name = "Food",
                Description = "Food specials, appetizers, and meal deals",
                Icon = "🍔",
                SortOrder = 1,
            },
            new SpecialCategory
            {
                Id = 2,
                Name = "Drink",
                Description = "Drink specials, happy hours, and beverage promotions",
                Icon = "🍺",
                SortOrder = 2,
            },
            new SpecialCategory
            {
                Id = 3,
                Name = "Entertainment",
                Description = "Live music, DJs, trivia, karaoke, and other events",
                Icon = "🎵",
                SortOrder = 3,
            }
        );
        #endregion
    }
}
