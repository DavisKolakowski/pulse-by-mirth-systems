using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;
using NodaTime;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for Role entity
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        #region Entity Configuration
        builder.ToTable("roles");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(r => r.DisplayName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(r => r.Description)
               .HasMaxLength(500);

        builder.HasIndex(r => r.Name)
               .IsUnique();

        builder.HasIndex(r => r.IsActive);
        #endregion

        #region Data Seed
        builder.HasData(
            new Role
            {
                Id = 1,
                Name = "administrator",
                DisplayName = "Administrator",
                Description = "Full global application administration access with complete system control",

                IsActive = true,
                SortOrder = 1,
                CreatedAt = NodaConstants.UnixEpoch
            },
            new Role
            {
                Id = 2,
                Name = "content-manager",
                DisplayName = "Content Manager",
                Description = "Manage all venues, content, and platform-wide moderation",

                IsActive = true,
                SortOrder = 2,
                CreatedAt = NodaConstants.UnixEpoch
            },
            new Role
            {
                Id = 3,
                Name = "venue-owner",
                DisplayName = "Venue Owner",
                Description = "Full management access for assigned venues including user management",

                IsActive = true,
                SortOrder = 3,
                CreatedAt = NodaConstants.UnixEpoch
            },
            new Role
            {
                Id = 4,
                Name = "venue-manager",
                DisplayName = "Venue Manager",
                Description = "Manage specials and content for assigned venues",

                IsActive = true,
                SortOrder = 4,
                CreatedAt = NodaConstants.UnixEpoch
            }
        );
        #endregion
    }
}
