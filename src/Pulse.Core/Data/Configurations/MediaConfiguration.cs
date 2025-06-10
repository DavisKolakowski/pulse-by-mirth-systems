using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for Media entity
/// </summary>
public class MediaConfiguration : IEntityTypeConfiguration<Media>
{    public void Configure(EntityTypeBuilder<Media> builder)
    {
        #region Entity Configuration
        builder.ToTable("media", t =>
        {
            t.HasCheckConstraint("CK_Media_Association",
                "(venue_id IS NOT NULL AND post_id IS NULL) OR (venue_id IS NULL AND post_id IS NOT NULL)");
        });
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FileName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(m => m.FilePath)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(m => m.ContentType)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(m => m.AltText)
               .HasMaxLength(200);

        builder.Property(m => m.CreatedByUserId)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(m => m.DeletedByUserId)
               .HasMaxLength(255);

        builder.HasOne(m => m.Venue)
               .WithMany(v => v.MediaFiles)
               .HasForeignKey(m => m.VenueId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Post)
               .WithMany(p => p.MediaAttachments)
               .HasForeignKey(m => m.PostId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(m => m.PostId);
        builder.HasIndex(m => m.MediaType);
        builder.HasIndex(m => m.CreatedAt);
        #endregion
    }
}
