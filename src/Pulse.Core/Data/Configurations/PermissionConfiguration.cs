using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;
using Pulse.Core.Authorization;
using NodaTime;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for Permission entity
/// </summary>
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        #region Entity Configuration
        builder.ToTable("permissions");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.DisplayName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Description)
               .HasMaxLength(500);

        builder.Property(p => p.Category)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(p => p.Action)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(p => p.Resource)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(p => p.Name)
               .IsUnique();

        builder.HasIndex(p => new { p.Category, p.Action, p.Resource })
               .IsUnique();

        builder.HasIndex(p => p.IsActive);
        #endregion

        #region Data Seed
        builder.HasData(
            // Venue Management Permissions
            new Permission { Id = 1, Name = "read:venues", DisplayName = "Read Venues", Description = "Query and view venue information", Category = PermissionCategories.Venue, Action = PermissionActions.Read, Resource = PermissionResources.Venues, SortOrder = 1, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 2, Name = "write:venues", DisplayName = "Write Venues", Description = "Create and update venue information", Category = PermissionCategories.Venue, Action = PermissionActions.Write, Resource = PermissionResources.Venues, SortOrder = 2, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 3, Name = "delete:venues", DisplayName = "Delete Venues", Description = "Delete venues from the system", Category = PermissionCategories.Venue, Action = PermissionActions.Delete, Resource = PermissionResources.Venues, SortOrder = 3, CreatedAt = NodaConstants.UnixEpoch },

            // Special Management Permissions
            new Permission { Id = 4, Name = "write:specials", DisplayName = "Write Specials", Description = "Create and update special offers", Category = PermissionCategories.Special, Action = PermissionActions.Write, Resource = PermissionResources.Specials, SortOrder = 4, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 5, Name = "delete:specials", DisplayName = "Delete Specials", Description = "Delete special offers", Category = PermissionCategories.Special, Action = PermissionActions.Delete, Resource = PermissionResources.Specials, SortOrder = 5, CreatedAt = NodaConstants.UnixEpoch },

            // Content Management Permissions
            new Permission { Id = 6, Name = "read:content", DisplayName = "Read Content", Description = "Read all user-generated content and posts", Category = PermissionCategories.Content, Action = PermissionActions.Read, Resource = PermissionResources.Content, SortOrder = 6, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 7, Name = "write:content", DisplayName = "Write Content", Description = "Create and update content across all venues", Category = PermissionCategories.Content, Action = PermissionActions.Write, Resource = PermissionResources.Content, SortOrder = 7, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 8, Name = "delete:content", DisplayName = "Delete Content", Description = "Delete inappropriate or violating content", Category = PermissionCategories.Content, Action = PermissionActions.Delete, Resource = PermissionResources.Content, SortOrder = 8, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 9, Name = "moderate:content", DisplayName = "Moderate Content", Description = "Moderate user posts and venue content", Category = PermissionCategories.Content, Action = PermissionActions.Moderate, Resource = PermissionResources.Content, SortOrder = 9, CreatedAt = NodaConstants.UnixEpoch },

            // Post Management Permissions
            new Permission { Id = 10, Name = "write:posts", DisplayName = "Write Posts", Description = "Create posts in venue activity threads", Category = PermissionCategories.Post, Action = PermissionActions.Write, Resource = PermissionResources.Posts, SortOrder = 10, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 11, Name = "delete:posts", DisplayName = "Delete Posts", Description = "Delete posts from activity threads", Category = PermissionCategories.Post, Action = PermissionActions.Delete, Resource = PermissionResources.Posts, SortOrder = 11, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 12, Name = "moderate:posts", DisplayName = "Moderate Posts", Description = "Moderate posts and manage thread activity", Category = PermissionCategories.Post, Action = PermissionActions.Moderate, Resource = PermissionResources.Posts, SortOrder = 12, CreatedAt = NodaConstants.UnixEpoch },

            // Media Management Permissions
            new Permission { Id = 13, Name = "upload:media", DisplayName = "Upload Media", Description = "Upload photos and videos to venue profiles and posts", Category = PermissionCategories.Media, Action = PermissionActions.Upload, Resource = PermissionResources.Media, SortOrder = 13, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 14, Name = "delete:media", DisplayName = "Delete Media", Description = "Delete media content", Category = PermissionCategories.Media, Action = PermissionActions.Delete, Resource = PermissionResources.Media, SortOrder = 14, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 15, Name = "moderate:media", DisplayName = "Moderate Media", Description = "Moderate media content for appropriateness", Category = PermissionCategories.Media, Action = PermissionActions.Moderate, Resource = PermissionResources.Media, SortOrder = 15, CreatedAt = NodaConstants.UnixEpoch },

            // Tag Management Permissions
            new Permission { Id = 16, Name = "read:tags", DisplayName = "Read Tags", Description = "Read tag definitions and assignments", Category = PermissionCategories.Tag, Action = PermissionActions.Read, Resource = PermissionResources.Tags, SortOrder = 16, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 17, Name = "write:tags", DisplayName = "Write Tags", Description = "Create and update tags for specials", Category = PermissionCategories.Tag, Action = PermissionActions.Write, Resource = PermissionResources.Tags, SortOrder = 17, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 18, Name = "delete:tags", DisplayName = "Delete Tags", Description = "Delete or consolidate tags", Category = PermissionCategories.Tag, Action = PermissionActions.Delete, Resource = PermissionResources.Tags, SortOrder = 18, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 19, Name = "moderate:tags", DisplayName = "Moderate Tags", Description = "Feature, hide, or manage tag usage across platform", Category = PermissionCategories.Tag, Action = PermissionActions.Moderate, Resource = PermissionResources.Tags, SortOrder = 19, CreatedAt = NodaConstants.UnixEpoch },

            // Vibe Management Permissions
            new Permission { Id = 20, Name = "read:vibes", DisplayName = "Read Vibes", Description = "Read vibe definitions and current venue vibes", Category = PermissionCategories.Vibe, Action = PermissionActions.Read, Resource = PermissionResources.Vibes, SortOrder = 20, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 21, Name = "write:vibes", DisplayName = "Write Vibes", Description = "Create vibes in user posts", Category = PermissionCategories.Vibe, Action = PermissionActions.Write, Resource = PermissionResources.Vibes, SortOrder = 21, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 22, Name = "moderate:vibes", DisplayName = "Moderate Vibes", Description = "Moderate vibe content for appropriateness", Category = PermissionCategories.Vibe, Action = PermissionActions.Moderate, Resource = PermissionResources.Vibes, SortOrder = 22, CreatedAt = NodaConstants.UnixEpoch },

            // Venue Category Management Permissions
            new Permission { Id = 23, Name = "read:venue-categories", DisplayName = "Read Venue Categories", Description = "Read available venue category classifications", Category = PermissionCategories.VenueCategory, Action = PermissionActions.Read, Resource = PermissionResources.VenueCategories, SortOrder = 23, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 24, Name = "write:venue-categories", DisplayName = "Write Venue Categories", Description = "Create and update venue category definitions", Category = PermissionCategories.VenueCategory, Action = PermissionActions.Write, Resource = PermissionResources.VenueCategories, SortOrder = 24, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 25, Name = "delete:venue-categories", DisplayName = "Delete Venue Categories", Description = "Remove venue categories from the system", Category = PermissionCategories.VenueCategory, Action = PermissionActions.Delete, Resource = PermissionResources.VenueCategories, SortOrder = 25, CreatedAt = NodaConstants.UnixEpoch },

            // Analytics Permissions
            new Permission { Id = 26, Name = "read:analytics", DisplayName = "Read Analytics", Description = "Access venue analytics and performance metrics", Category = PermissionCategories.Analytics, Action = PermissionActions.Read, Resource = PermissionResources.Analytics, SortOrder = 26, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 27, Name = "read:analytics-global", DisplayName = "Global Analytics", Description = "Access global platform analytics and insights", Category = PermissionCategories.Analytics, Action = PermissionActions.Read, Resource = PermissionResources.AnalyticsGlobal, SortOrder = 27, CreatedAt = NodaConstants.UnixEpoch },

            // Notification Management Permissions
            new Permission { Id = 28, Name = "read:notifications", DisplayName = "Read Notifications", Description = "Read user notifications", Category = PermissionCategories.Notification, Action = PermissionActions.Read, Resource = PermissionResources.Notifications, SortOrder = 28, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 29, Name = "write:notifications", DisplayName = "Write Notifications", Description = "Send notifications to users", Category = PermissionCategories.Notification, Action = PermissionActions.Write, Resource = PermissionResources.Notifications, SortOrder = 29, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 30, Name = "manage:follows", DisplayName = "Manage Follows", Description = "Follow/unfollow tags and venues for notifications", Category = PermissionCategories.Follow, Action = PermissionActions.Manage, Resource = PermissionResources.Follows, SortOrder = 30, CreatedAt = NodaConstants.UnixEpoch },

            // User Management Permissions
            new Permission { Id = 31, Name = "read:venue-users", DisplayName = "Read Venue Users", Description = "Read venue user assignments and permissions", Category = PermissionCategories.VenueUser, Action = PermissionActions.Read, Resource = PermissionResources.VenueUsers, SortOrder = 31, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 32, Name = "write:venue-users", DisplayName = "Write Venue Users", Description = "Assign users to venues and manage venue-specific permissions", Category = PermissionCategories.VenueUser, Action = PermissionActions.Write, Resource = PermissionResources.VenueUsers, SortOrder = 32, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 33, Name = "delete:venue-users", DisplayName = "Delete Venue Users", Description = "Remove users from venue assignments", Category = PermissionCategories.VenueUser, Action = PermissionActions.Delete, Resource = PermissionResources.VenueUsers, SortOrder = 33, CreatedAt = NodaConstants.UnixEpoch },

            // System Administration Permissions
            new Permission { Id = 34, Name = "admin:system", DisplayName = "System Admin", Description = "Full system administration access", Category = PermissionCategories.System, Action = PermissionActions.Admin, Resource = PermissionResources.System, SortOrder = 34, CreatedAt = NodaConstants.UnixEpoch },
            new Permission { Id = 35, Name = "config:system", DisplayName = "System Config", Description = "Modify system configuration and settings", Category = PermissionCategories.System, Action = PermissionActions.Config, Resource = PermissionResources.System, SortOrder = 35, CreatedAt = NodaConstants.UnixEpoch }
        );
        #endregion
    }
}
