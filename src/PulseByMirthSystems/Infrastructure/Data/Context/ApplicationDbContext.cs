using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PulseByMirthSystems.Domain.Entities;

namespace PulseByMirthSystems.Infrastructure.Data.Context;

/// <summary>
/// Application database context extending IdentityDbContext for complete Identity integration
/// </summary>
public class ApplicationDbContext : IdentityDbContext<
    ApplicationUser,           // TUser
    ApplicationRole,           // TRole  
    long,                      // TKey
    ApplicationUserClaim,      // TUserClaim
    ApplicationUserRole,       // TUserRole
    ApplicationUserLogin,      // TUserLogin
    ApplicationRoleClaim,      // TRoleClaim
    ApplicationUserToken>      // TUserToken
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasPostgresExtension("address_standardizer");
        builder.HasPostgresExtension("address_standardizer_data_us");
        builder.HasPostgresExtension("fuzzystrmatch");
        builder.HasPostgresExtension("plpgsql");
        builder.HasPostgresExtension("postgis");
        builder.HasPostgresExtension("postgis_raster");
        builder.HasPostgresExtension("postgis_sfcgal");
        builder.HasPostgresExtension("postgis_tiger_geocoder");
        builder.HasPostgresExtension("postgis_topology");

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
