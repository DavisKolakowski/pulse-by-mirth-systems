using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseByMirthSystems.Domain.Entities;

namespace PulseByMirthSystems.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for ApplicationRoleClaim entity.
/// Configures PostgreSQL-specific settings for role claims.
/// </summary>
public class ApplicationRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
    {
        builder.HasKey(rc => rc.Id);

        builder.ToTable("application_role_claims");

        builder.Property(rc => rc.Id)
            .HasColumnName("id");
        
        builder.Property(rc => rc.RoleId)
            .HasColumnName("role_id");
        
        builder.Property(rc => rc.ClaimType)
            .HasColumnName("claim_type");
        
        builder.Property(rc => rc.ClaimValue)
            .HasColumnName("claim_value");
    }
}
