using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for UserRole entity
/// </summary>
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        #region Entity Configuration
        builder.ToTable("user_roles");
        builder.HasKey(ur => ur.Id);

        builder.HasOne(ur => ur.User)
               .WithMany(u => u.UserRoles)
               .HasForeignKey(ur => ur.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.Role)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(ur => ur.RoleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.AssignedBy)
               .WithMany()
               .HasForeignKey(ur => ur.AssignedByUserId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(ur => ur.DeactivatedBy)
               .WithMany()
               .HasForeignKey(ur => ur.DeactivatedByUserId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
               .IsUnique()
               .HasFilter("is_active = true");
       builder.HasIndex(ur => ur.UserId)
               .HasFilter("is_active = true");
        #endregion

        #region Data Seed
        builder.HasData(
            new UserRole
            {
                Id = 1,
                UserId = 1, // Davis Kolakowski
                RoleId = 1, // Administrator role
                AssignedAt = NodaConstants.UnixEpoch,
                IsActive = true
            }
        );
        #endregion
    }
}
