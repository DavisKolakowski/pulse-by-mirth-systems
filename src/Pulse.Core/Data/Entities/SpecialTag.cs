namespace Pulse.Core.Data.Entities;

/// <summary>
/// Many-to-many relationship between Specials and Tags
/// </summary>
public class SpecialTag
{
    #region Identity and primary fields
    public long SpecialId { get; set; }
    public long TagId { get; set; }
    #endregion

    #region Navigation properties
    public Special? Special { get; set; }
    public Tag? Tag { get; set; }
    #endregion
}
