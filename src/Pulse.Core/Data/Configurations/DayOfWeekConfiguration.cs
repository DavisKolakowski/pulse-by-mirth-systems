using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pulse.Core.Data.Entities;
using DayOfWeek = Pulse.Core.Data.Entities.DayOfWeek;

namespace Pulse.Core.Data.Configurations;

/// <summary>
/// Entity configuration for DayOfWeek entity
/// </summary>
public class DayOfWeekConfiguration : IEntityTypeConfiguration<DayOfWeek>
{
    public void Configure(EntityTypeBuilder<DayOfWeek> builder)
    {
        #region Entity Configuration
        builder.ToTable("days_of_week");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(d => d.ShortName)
               .IsRequired()
               .HasMaxLength(3);
        builder.HasIndex(d => d.IsoNumber)
               .IsUnique();
        #endregion

        #region Data Seed
        builder.HasData(new DayOfWeek
            {
                Id = 1,
                Name = "Sunday",
                ShortName = "SUN",
                IsoNumber = 0,
                IsWeekday = false,
                SortOrder = 1
            },
            new DayOfWeek
            {
                Id = 2,
                Name = "Monday",
                ShortName = "MON",
                IsoNumber = 1,
                IsWeekday = true,
                SortOrder = 2
            },
            new DayOfWeek
            {
                Id = 3,
                Name = "Tuesday",
                ShortName = "TUE",
                IsoNumber = 2,
                IsWeekday = true,
                SortOrder = 3
            },
            new DayOfWeek
            {
                Id = 4,
                Name = "Wednesday",
                ShortName = "WED",
                IsoNumber = 3,
                IsWeekday = true,
                SortOrder = 4
            },
            new DayOfWeek
            {
                Id = 5,
                Name = "Thursday",
                ShortName = "THU",
                IsoNumber = 4,
                IsWeekday = true,
                SortOrder = 5
            },
            new DayOfWeek
            {
                Id = 6,
                Name = "Friday",
                ShortName = "FRI",
                IsoNumber = 5,
                IsWeekday = true,
                SortOrder = 6
            },
            new DayOfWeek
            {
                Id = 7,
                Name = "Saturday",
                ShortName = "SAT",
                IsoNumber = 6,
                IsWeekday = false,
                SortOrder = 7
            }
        );
        #endregion
    }
}
