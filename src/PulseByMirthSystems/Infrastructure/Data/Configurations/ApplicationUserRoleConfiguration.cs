using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseByMirthSystems.Domain.Entities;

namespace PulseByMirthSystems.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for ApplicationUserRole entity.
/// Configures PostgreSQL-specific settings for the many-to-many relationship between users and roles.
/// </summary>
public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.HasKey(r => new { r.UserId, r.RoleId });

        builder.ToTable("application_user_roles");

        builder.Property(ur => ur.UserId)
            .HasColumnName("user_id");
        
        builder.Property(ur => ur.RoleId)
            .HasColumnName("role_id");
    }
}
