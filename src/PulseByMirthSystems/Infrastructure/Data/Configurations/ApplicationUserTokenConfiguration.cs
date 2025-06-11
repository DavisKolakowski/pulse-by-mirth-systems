using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseByMirthSystems.Domain.Entities;

namespace PulseByMirthSystems.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for ApplicationUserToken entity.
/// Configures PostgreSQL-specific settings for user authentication tokens.
/// </summary>
public class ApplicationUserTokenConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
{
    public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
    {
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        builder.ToTable("application_user_tokens");

        builder.Property(t => t.LoginProvider)
            .HasMaxLength(128);
        
        builder.Property(t => t.Name)
            .HasMaxLength(128);

        builder.Property(ut => ut.UserId)
            .HasColumnName("user_id");
        
        builder.Property(ut => ut.LoginProvider)
            .HasColumnName("login_provider");
        
        builder.Property(ut => ut.Name)
            .HasColumnName("name");
        
        builder.Property(ut => ut.Value)
            .HasColumnName("value");
    }
}
