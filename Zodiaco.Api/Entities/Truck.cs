using Zodiaco.Api.Common;

namespace Zodiaco.Api.Entities;

public class Truck
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Slug { get; set; } = string.Empty;
    public string NormalizedSlug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Mileage { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = TruckCurrencyValues.Mxn;
    public string Type { get; set; } = string.Empty;
    public string? Transmission { get; set; }
    public string? Engine { get; set; }
    public string? Configuration { get; set; }
    public string? InternalNumber { get; set; }
    public string? VinOrSerial { get; set; }
    public string? Color { get; set; }
    public string? DocumentationStatus { get; set; }
    public string? MechanicalCondition { get; set; }
    public string? CommercialObservations { get; set; }
    public bool PriceIncludesVat { get; set; } = true;
    public string? PaymentOptions { get; set; } = PaymentOptionValues.CashAndFinancing;
    public string LocationState { get; set; } = string.Empty;
    public string? LocationCity { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = TruckStatusValues.UnderReview;
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<TruckImage> Images { get; set; } = new List<TruckImage>();
    public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    public ICollection<QuoteRequest> QuoteRequests { get; set; } = new List<QuoteRequest>();
}
