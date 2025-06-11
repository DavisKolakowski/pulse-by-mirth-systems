using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseByMirthSystems.Domain.Entities;

namespace PulseByMirthSystems.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for ApplicationUser entity.
/// Configures PostgreSQL-specific settings, indexes, and relationships.
/// </summary>
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(u => u.Id);

        builder.ToTable("application_users");

        builder.HasIndex(u => u.NormalizedUserName)
            .HasDatabaseName("user_name_index")
            .IsUnique();
        
        builder.HasIndex(u => u.NormalizedEmail)
            .HasDatabaseName("email_index");

        builder.Property(u => u.ConcurrencyStamp)
            .IsConcurrencyToken();

        builder.Property(u => u.UserName)
            .HasMaxLength(256);
        
        builder.Property(u => u.NormalizedUserName)
            .HasMaxLength(256);
        
        builder.Property(u => u.Email)
            .HasMaxLength(256);
        
        builder.Property(u => u.NormalizedEmail)
            .HasMaxLength(256);

        builder.Property(u => u.Id)
            .HasColumnName("id");
        
        builder.Property(u => u.UserName)
            .HasColumnName("user_name");
        
        builder.Property(u => u.NormalizedUserName)
            .HasColumnName("normalized_user_name");
        
        builder.Property(u => u.Email)
            .HasColumnName("email");
        
        builder.Property(u => u.NormalizedEmail)
            .HasColumnName("normalized_email");
        
        builder.Property(u => u.EmailConfirmed)
            .HasColumnName("email_confirmed");
        
        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash");
        
        builder.Property(u => u.SecurityStamp)
            .HasColumnName("security_stamp");
        
        builder.Property(u => u.ConcurrencyStamp)
            .HasColumnName("concurrency_stamp");
        
        builder.Property(u => u.PhoneNumber)
            .HasColumnName("phone_number");
        
        builder.Property(u => u.PhoneNumberConfirmed)
            .HasColumnName("phone_number_confirmed");
        
        builder.Property(u => u.TwoFactorEnabled)
            .HasColumnName("two_factor_enabled");
        
        builder.Property(u => u.LockoutEnd)
            .HasColumnName("lockout_end");
        
        builder.Property(u => u.LockoutEnabled)
            .HasColumnName("lockout_enabled");
        
        builder.Property(u => u.AccessFailedCount)
            .HasColumnName("access_failed_count");

        builder.HasMany(e => e.Claims)
            .WithOne(e => e.User)
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

        builder.HasMany(e => e.Logins)
            .WithOne(e => e.User)
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();

        builder.HasMany(e => e.Tokens)
            .WithOne(e => e.User)
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}
