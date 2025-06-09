using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for SpecialTag entity
/// </summary>
public class SpecialTagConfiguration : IEntityTypeConfiguration<SpecialTag>
{
    public void Configure(EntityTypeBuilder<SpecialTag> builder)
    {
        #region Entity Configuration
        builder.ToTable("special_tags");
        builder.HasKey(st => new { st.SpecialId, st.TagId });

        builder.HasOne(st => st.Special)
               .WithMany(s => s.SpecialTags)
               .HasForeignKey(st => st.SpecialId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(st => st.Tag)
               .WithMany(t => t.SpecialTags)
               .HasForeignKey(st => st.TagId)
               .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
