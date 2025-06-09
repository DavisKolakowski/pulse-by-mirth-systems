using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for ActivityThread entity
/// </summary>
public class ActivityThreadConfiguration : IEntityTypeConfiguration<ActivityThread>
{
    public void Configure(EntityTypeBuilder<ActivityThread> builder)
    {
        #region Entity Configuration
        builder.ToTable("activity_threads");
        builder.HasKey(at => at.Id);

        builder.Property(at => at.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(at => at.Description)
               .HasMaxLength(500);

        builder.Property(at => at.CreatedByUserId)
               .IsRequired()
               .HasMaxLength(255);

        builder.HasOne(at => at.Venue)
               .WithMany(v => v.ActivityThreads)
               .HasForeignKey(at => at.VenueId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(at => at.VenueId);
        builder.HasIndex(at => at.ExpiresAt);
        builder.HasIndex(at => new { at.VenueId, at.IsActive });
        #endregion
    }
}
