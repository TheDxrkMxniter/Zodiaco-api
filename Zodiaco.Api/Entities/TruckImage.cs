namespace Zodiaco.Api.Entities;

public class TruckImage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TruckId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Alt { get; set; }
    public int SortOrder { get; set; }
    public bool IsCover { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Truck Truck { get; set; } = null!;
}
