using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for PostVibe entity
/// </summary>
public class PostVibeConfiguration : IEntityTypeConfiguration<PostVibe>
{
    public void Configure(EntityTypeBuilder<PostVibe> builder)
    {
        #region Entity Configuration
        builder.ToTable("post_vibes");
        builder.HasKey(pv => new { pv.PostId, pv.VibeId });

        builder.HasOne(pv => pv.Post)
               .WithMany(p => p.PostVibes)
               .HasForeignKey(pv => pv.PostId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pv => pv.Vibe)
               .WithMany(v => v.PostVibes)
               .HasForeignKey(pv => pv.VibeId)
               .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
