using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;
namespace Pulse.Core.Data.Configurations;
/// <summary>
/// Entity configuration for VenueCategory entity
/// </summary>
public class VenueCategoryConfiguration : IEntityTypeConfiguration<VenueCategory>
{
    public void Configure(EntityTypeBuilder<VenueCategory> builder)
    {
        #region Entity Configuration
        builder.ToTable("venue_categories");
        builder.HasKey(vc => vc.Id);
        builder.Property(vc => vc.Name)
               .IsRequired()
               .HasMaxLength(50);
        builder.Property(vc => vc.Description)
               .HasMaxLength(200);
        builder.Property(vc => vc.Icon)
               .HasMaxLength(10);
        builder.HasMany(vc => vc.Venues)
               .WithOne(v => v.Category)
               .HasForeignKey(v => v.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(vc => vc.Name)
               .IsUnique();
        #endregion

        #region Data Seed
        builder.HasData(
            new VenueCategory 
            { 
                Id = 1, 
                Name = "Restaurant", 
                Description = "Dining establishments offering food and beverages", 
                Icon = "🍽️",
                SortOrder = 1,
            },
            new VenueCategory 
            { 
                Id = 2, 
                Name = "Bar", 
                Description = "Venues focused on drinks and nightlife", 
                Icon = "🍸",
                SortOrder = 2,
            },
            new VenueCategory 
            { 
                Id = 3, 
                Name = "Cafe", 
                Description = "Casual spots for coffee and light meals", 
                Icon = "☕",
                SortOrder = 3,
            },
            new VenueCategory 
            { 
                Id = 4, 
                Name = "Nightclub", 
                Description = "Venues for dancing and late-night entertainment", 
                Icon = "🪩",
                SortOrder = 4,
            },
            new VenueCategory 
            { 
                Id = 5, 
                Name = "Pub", 
                Description = "Casual venues with food, drinks, and often live music", 
                Icon = "🍺",
                SortOrder = 5,
            },
            new VenueCategory 
            { 
                Id = 6, 
                Name = "Winery", 
                Description = "Venues producing wine, offering tastings, food pairings, and live music", 
                Icon = "🍷",
                SortOrder = 6,
            },
            new VenueCategory 
            { 
                Id = 7, 
                Name = "Brewery", 
                Description = "Venues brewing their own beer, often with food and live music", 
                Icon = "🍻",
                SortOrder = 7,
            },
            new VenueCategory 
            { 
                Id = 9, 
                Name = "Lounge", 
                Description = "Sophisticated venues with cocktails, small plates, and live music", 
                Icon = "🛋️",
                SortOrder = 8,
            },
            new VenueCategory 
            { 
                Id = 10, 
                Name = "Bistro", 
                Description = "Intimate dining venues with quality food, wine, and occasional live music", 
                Icon = "🥂",
                SortOrder = 9,
            }
        );
        #endregion
    }
}
