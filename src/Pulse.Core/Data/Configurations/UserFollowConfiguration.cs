using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for UserFollow entity
/// </summary>
public class UserFollowConfiguration : IEntityTypeConfiguration<UserFollow>
{
    public void Configure(EntityTypeBuilder<UserFollow> builder)
    {
        #region Entity Configuration
        builder.ToTable("user_follows");
        builder.HasKey(uf => uf.Id);

        builder.HasOne(uf => uf.User)
               .WithMany(u => u.Follows)
               .HasForeignKey(uf => uf.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uf => uf.Tag)
               .WithMany(t => t.Followers)
               .HasForeignKey(uf => uf.TagId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uf => uf.Venue)
               .WithMany(v => v.Followers)
               .HasForeignKey(uf => uf.VenueId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(uf => new { uf.UserId, uf.FollowType });
        builder.HasIndex(uf => new { uf.TagId, uf.IsActive });
        builder.HasIndex(uf => new { uf.VenueId, uf.IsActive });

        // Ensure follow is associated with either a tag or a venue, but not both
        builder.HasCheckConstraint("CK_UserFollow_Association", 
            "(follow_type = 1 AND tag_id IS NOT NULL AND venue_id IS NULL) OR (follow_type = 2 AND tag_id IS NULL AND venue_id IS NOT NULL)");

        // Unique constraint for active follows
        builder.HasIndex(uf => new { uf.UserId, uf.TagId, uf.FollowType })
               .IsUnique()
               .HasFilter("is_active = true AND tag_id IS NOT NULL");

        builder.HasIndex(uf => new { uf.UserId, uf.VenueId, uf.FollowType })
               .IsUnique()
               .HasFilter("is_active = true AND venue_id IS NOT NULL");
        #endregion
    }
}
