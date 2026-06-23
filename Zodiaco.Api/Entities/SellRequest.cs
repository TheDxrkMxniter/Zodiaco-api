using Zodiaco.Api.Common;

namespace Zodiaco.Api.Entities;

public class SellRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Company { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int? Year { get; set; }
    public int? Mileage { get; set; }
    public string? TruckType { get; set; }
    public string? LocationState { get; set; }
    public decimal? ExpectedPrice { get; set; }
    public string? DocumentationStatus { get; set; }
    public string? Comments { get; set; }
    public string Status { get; set; } = LeadStatusValues.PendingReview;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
