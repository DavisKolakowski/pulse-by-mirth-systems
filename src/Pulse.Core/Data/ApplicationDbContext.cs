using Microsoft.EntityFrameworkCore;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data;

/// <summary>
/// Application database context for the Pulse application
/// </summary>
public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    #region Core Entities
    public DbSet<User> Users => Set<User>();
    public DbSet<VenueRole> VenueRoles => Set<VenueRole>();
    #endregion

    #region Venue Entities
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<VenueCategory> VenueCategories => Set<VenueCategory>();
    public DbSet<BusinessHours> BusinessHours => Set<BusinessHours>();
    public DbSet<DayOfWeek> DayOfWeeks => Set<DayOfWeek>();
    #endregion

    #region Special Entities
    public DbSet<Special> Specials => Set<Special>(); 
    public DbSet<SpecialCategory> SpecialCategories => Set<SpecialCategory>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<SpecialTag> SpecialTags => Set<SpecialTag>();
    #endregion

    #region Activity and Content Entities
    public DbSet<ActivityThread> ActivityThreads => Set<ActivityThread>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Vibe> Vibes => Set<Vibe>();
    public DbSet<PostVibe> PostVibes => Set<PostVibe>();
    public DbSet<Media> MediaFiles => Set<Media>();
    #endregion

    #region Notification and Following Entities
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<UserFollow> UserFollows => Set<UserFollow>();
    #endregion
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {           
        base.OnModelCreating(modelBuilder);

        #region PostgreSQL Extensions
        modelBuilder.HasPostgresExtension("address_standardizer");
        modelBuilder.HasPostgresExtension("address_standardizer_data_us");
        modelBuilder.HasPostgresExtension("fuzzystrmatch");
        modelBuilder.HasPostgresExtension("plpgsql");
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.HasPostgresExtension("postgis_raster");
        modelBuilder.HasPostgresExtension("postgis_sfcgal");
        modelBuilder.HasPostgresExtension("postgis_tiger_geocoder");
        modelBuilder.HasPostgresExtension("postgis_topology");
        #endregion

        #region Apply Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        #endregion
    }
}
