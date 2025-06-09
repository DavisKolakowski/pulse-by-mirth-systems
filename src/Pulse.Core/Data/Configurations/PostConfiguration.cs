using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for Post entity
/// </summary>
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        #region Entity Configuration
        builder.ToTable("posts");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Content)
               .IsRequired()
               .HasMaxLength(1000);

        builder.HasOne(p => p.ActivityThread)
               .WithMany(at => at.Posts)
               .HasForeignKey(p => p.ActivityThreadId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.User)
               .WithMany(u => u.Posts)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.DeletedBy)
               .WithMany()
               .HasForeignKey(p => p.DeletedByUserId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(p => p.ActivityThreadId);
        builder.HasIndex(p => p.UserId);
        builder.HasIndex(p => p.CreatedAt);
        builder.HasIndex(p => new { p.ActivityThreadId, p.IsActive });
        #endregion
    }
}
