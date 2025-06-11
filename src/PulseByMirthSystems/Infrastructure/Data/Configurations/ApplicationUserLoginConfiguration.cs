using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseByMirthSystems.Domain.Entities;

namespace PulseByMirthSystems.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for ApplicationUserLogin entity.
/// Configures PostgreSQL-specific settings for external login providers.
/// </summary>
public class ApplicationUserLoginConfiguration : IEntityTypeConfiguration<ApplicationUserLogin>
{
    public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
    {
        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

        builder.ToTable("application_user_logins");

        builder.Property(l => l.LoginProvider)
            .HasMaxLength(128);
        
        builder.Property(l => l.ProviderKey)
            .HasMaxLength(128);

        builder.Property(ul => ul.LoginProvider)
            .HasColumnName("login_provider");
        
        builder.Property(ul => ul.ProviderKey)
            .HasColumnName("provider_key");
        
        builder.Property(ul => ul.ProviderDisplayName)
            .HasColumnName("provider_display_name");
        
        builder.Property(ul => ul.UserId)
            .HasColumnName("user_id");
    }
}
