using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for VenueRole entity
/// </summary>
public class VenueRoleConfiguration : IEntityTypeConfiguration<VenueRole>
{
    public void Configure(EntityTypeBuilder<VenueRole> builder)
    {
        #region Entity Configuration
        builder.ToTable("venue_roles");
        builder.HasKey(vr => vr.Id);
        builder.Property(vr => vr.RoleId)
               .IsRequired();

        builder.HasOne(vr => vr.User)
               .WithMany(u => u.VenueRoles)
               .HasForeignKey(vr => vr.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(vr => vr.Venue)
               .WithMany(v => v.VenueRoles)
               .HasForeignKey(vr => vr.VenueId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(vr => vr.Role)
               .WithMany(r => r.VenueRoles)
               .HasForeignKey(vr => vr.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vr => vr.AssignedBy)
               .WithMany()
               .HasForeignKey(vr => vr.AssignedByUserId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(vr => vr.DeactivatedBy)
               .WithMany()
               .HasForeignKey(vr => vr.DeactivatedByUserId)
               .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(vr => new { vr.UserId, vr.VenueId, vr.RoleId })
               .IsUnique()
               .HasFilter("is_active = true");

        builder.HasIndex(vr => new { vr.VenueId, vr.RoleId })
               .HasFilter("is_active = true");
        #endregion
    }
}
