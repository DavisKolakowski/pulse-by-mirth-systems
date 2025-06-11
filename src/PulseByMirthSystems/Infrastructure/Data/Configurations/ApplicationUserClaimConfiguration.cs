using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseByMirthSystems.Domain.Entities;

namespace PulseByMirthSystems.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for ApplicationUserClaim entity.
/// Configures PostgreSQL-specific settings for user claims.
/// </summary>
public class ApplicationUserClaimConfiguration : IEntityTypeConfiguration<ApplicationUserClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
    {
        builder.HasKey(uc => uc.Id);

        builder.ToTable("application_user_claims");

        builder.Property(uc => uc.Id)
            .HasColumnName("id");
        
        builder.Property(uc => uc.UserId)
            .HasColumnName("user_id");
        
        builder.Property(uc => uc.ClaimType)
            .HasColumnName("claim_type");
        
        builder.Property(uc => uc.ClaimValue)
            .HasColumnName("claim_value");
    }
}
