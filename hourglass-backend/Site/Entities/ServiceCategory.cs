using System.ComponentModel.DataAnnotations.Schema;

namespace Hourglass.Site.Entities;

public class ServiceCategory
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public string Name { get; set; } 

    #region CreatedAt
    [NotMapped]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTime CreatedAtDate
    {
        get => CreatedAt.UtcDateTime;
        set => CreatedAt = new DateTimeOffset(value, TimeSpan.Zero);
    }
	#endregion

	#region UpdatedAt
	[NotMapped]
	public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTime UpdatedAtDate
    {
        get => UpdatedAt.UtcDateTime;
        set => UpdatedAt = new DateTimeOffset(value, TimeSpan.Zero);
    }
    #endregion
}
