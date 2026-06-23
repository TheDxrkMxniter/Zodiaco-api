using Zodiaco.Api.Common;

namespace Zodiaco.Api.Entities;

public class QuoteRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? TruckId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Company { get; set; }
    public string? Message { get; set; }
    public string Status { get; set; } = LeadStatusValues.PendingReview;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Truck? Truck { get; set; }
}
