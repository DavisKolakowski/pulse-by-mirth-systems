using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents business hours for a venue on a specific day of the week
/// </summary>
public class BusinessHours
{
    #region Identity and primary fields
    public long Id { get; set; }
    public long VenueId { get; set; }
    public int DayOfWeekId { get; set; }
    #endregion

    #region Operating time information
    public LocalTime? OpenTime { get; set; }
    public LocalTime? CloseTime { get; set; }
    public bool IsClosed { get; set; }
    #endregion

    #region Navigation properties
    public Venue? Venue { get; set; }
    public DayOfWeek? DayOfWeek { get; set; }
    #endregion
}
