using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for RolePermission entity
/// </summary>
public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        #region Entity Configuration
        builder.ToTable("role_permissions");
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.HasOne(rp => rp.Role)
               .WithMany(r => r.RolePermissions)
               .HasForeignKey(rp => rp.RoleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.Permission)
               .WithMany(p => p.RolePermissions)
               .HasForeignKey(rp => rp.PermissionId)
               .OnDelete(DeleteBehavior.Cascade);
        #endregion

        #region Data Seed - Role Permission Assignments
        builder.HasData(
            // Administrator Role (ID: 1) - All Permissions
            new RolePermission { RoleId = 1, PermissionId = 1 },   // read:venues
            new RolePermission { RoleId = 1, PermissionId = 2 },   // write:venues
            new RolePermission { RoleId = 1, PermissionId = 3 },   // delete:venues
            new RolePermission { RoleId = 1, PermissionId = 4 },   // write:specials
            new RolePermission { RoleId = 1, PermissionId = 5 },   // delete:specials
            new RolePermission { RoleId = 1, PermissionId = 6 },   // read:content
            new RolePermission { RoleId = 1, PermissionId = 7 },   // write:content
            new RolePermission { RoleId = 1, PermissionId = 8 },   // delete:content
            new RolePermission { RoleId = 1, PermissionId = 9 },   // moderate:content
            new RolePermission { RoleId = 1, PermissionId = 10 },  // write:posts
            new RolePermission { RoleId = 1, PermissionId = 11 },  // delete:posts
            new RolePermission { RoleId = 1, PermissionId = 12 },  // moderate:posts
            new RolePermission { RoleId = 1, PermissionId = 13 },  // upload:media
            new RolePermission { RoleId = 1, PermissionId = 14 },  // delete:media
            new RolePermission { RoleId = 1, PermissionId = 15 },  // moderate:media
            new RolePermission { RoleId = 1, PermissionId = 16 },  // read:tags
            new RolePermission { RoleId = 1, PermissionId = 17 },  // write:tags
            new RolePermission { RoleId = 1, PermissionId = 18 },  // delete:tags
            new RolePermission { RoleId = 1, PermissionId = 19 },  // moderate:tags
            new RolePermission { RoleId = 1, PermissionId = 20 },  // read:vibes
            new RolePermission { RoleId = 1, PermissionId = 21 },  // write:vibes
            new RolePermission { RoleId = 1, PermissionId = 22 },  // moderate:vibes
            new RolePermission { RoleId = 1, PermissionId = 23 },  // read:venue-categories
            new RolePermission { RoleId = 1, PermissionId = 24 },  // write:venue-categories
            new RolePermission { RoleId = 1, PermissionId = 25 },  // delete:venue-categories
            new RolePermission { RoleId = 1, PermissionId = 26 },  // read:analytics
            new RolePermission { RoleId = 1, PermissionId = 27 },  // read:analytics-global
            new RolePermission { RoleId = 1, PermissionId = 28 },  // read:notifications
            new RolePermission { RoleId = 1, PermissionId = 29 },  // write:notifications
            new RolePermission { RoleId = 1, PermissionId = 30 },  // manage:follows
            new RolePermission { RoleId = 1, PermissionId = 31 },  // read:venue-users
            new RolePermission { RoleId = 1, PermissionId = 32 },  // write:venue-users
            new RolePermission { RoleId = 1, PermissionId = 33 },  // delete:venue-users
            new RolePermission { RoleId = 1, PermissionId = 34 },  // admin:system
            new RolePermission { RoleId = 1, PermissionId = 35 },  // config:system

            // Content Manager Role (ID: 2)
            new RolePermission { RoleId = 2, PermissionId = 1 },   // read:venues
            new RolePermission { RoleId = 2, PermissionId = 2 },   // write:venues
            new RolePermission { RoleId = 2, PermissionId = 4 },   // write:specials
            new RolePermission { RoleId = 2, PermissionId = 6 },   // read:content
            new RolePermission { RoleId = 2, PermissionId = 7 },   // write:content
            new RolePermission { RoleId = 2, PermissionId = 8 },   // delete:content
            new RolePermission { RoleId = 2, PermissionId = 9 },   // moderate:content
            new RolePermission { RoleId = 2, PermissionId = 10 },  // write:posts
            new RolePermission { RoleId = 2, PermissionId = 11 },  // delete:posts
            new RolePermission { RoleId = 2, PermissionId = 12 },  // moderate:posts
            new RolePermission { RoleId = 2, PermissionId = 13 },  // upload:media
            new RolePermission { RoleId = 2, PermissionId = 14 },  // delete:media
            new RolePermission { RoleId = 2, PermissionId = 15 },  // moderate:media
            new RolePermission { RoleId = 2, PermissionId = 16 },  // read:tags
            new RolePermission { RoleId = 2, PermissionId = 17 },  // write:tags
            new RolePermission { RoleId = 2, PermissionId = 19 },  // moderate:tags
            new RolePermission { RoleId = 2, PermissionId = 20 },  // read:vibes
            new RolePermission { RoleId = 2, PermissionId = 22 },  // moderate:vibes
            new RolePermission { RoleId = 2, PermissionId = 23 },  // read:venue-categories
            new RolePermission { RoleId = 2, PermissionId = 26 },  // read:analytics

            // Venue Owner Role (ID: 3) - Limited to assigned venues
            new RolePermission { RoleId = 3, PermissionId = 1 },   // read:venues (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 2 },   // write:venues (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 3 },   // delete:venues (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 4 },   // write:specials (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 5 },   // delete:specials (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 6 },   // read:content (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 7 },   // write:content (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 10 },  // write:posts (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 13 },  // upload:media (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 16 },  // read:tags
            new RolePermission { RoleId = 3, PermissionId = 17 },  // write:tags (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 20 },  // read:vibes
            new RolePermission { RoleId = 3, PermissionId = 21 },  // write:vibes (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 23 },  // read:venue-categories
            new RolePermission { RoleId = 3, PermissionId = 26 },  // read:analytics (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 28 },  // read:notifications
            new RolePermission { RoleId = 3, PermissionId = 29 },  // write:notifications (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 30 },  // manage:follows
            new RolePermission { RoleId = 3, PermissionId = 31 },  // read:venue-users (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 32 },  // write:venue-users (venue-scoped)
            new RolePermission { RoleId = 3, PermissionId = 33 },  // delete:venue-users (venue-scoped)

            // Venue Manager Role (ID: 4) - Limited to assigned venues
            new RolePermission { RoleId = 4, PermissionId = 1 },   // read:venues (venue-scoped)
            new RolePermission { RoleId = 4, PermissionId = 4 },   // write:specials (venue-scoped)
            new RolePermission { RoleId = 4, PermissionId = 5 },   // delete:specials (venue-scoped)
            new RolePermission { RoleId = 4, PermissionId = 6 },   // read:content (venue-scoped)
            new RolePermission { RoleId = 4, PermissionId = 7 },   // write:content (venue-scoped)
            new RolePermission { RoleId = 4, PermissionId = 10 },  // write:posts (venue-scoped)
            new RolePermission { RoleId = 4, PermissionId = 13 },  // upload:media (venue-scoped)
            new RolePermission { RoleId = 4, PermissionId = 16 },  // read:tags
            new RolePermission { RoleId = 4, PermissionId = 17 },  // write:tags (venue-scoped)
            new RolePermission { RoleId = 4, PermissionId = 20 },  // read:vibes
            new RolePermission { RoleId = 4, PermissionId = 21 },  // write:vibes (venue-scoped)
            new RolePermission { RoleId = 4, PermissionId = 23 },  // read:venue-categories
            new RolePermission { RoleId = 4, PermissionId = 26 },  // read:analytics (venue-scoped)
            new RolePermission { RoleId = 4, PermissionId = 28 }   // read:notifications
        );
        #endregion
    }
}
