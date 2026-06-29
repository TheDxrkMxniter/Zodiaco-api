namespace Zodiaco.Api.DTOs;

/// <summary>
/// Public truck detail returned for the truck detail page.
/// </summary>
public sealed class TruckDetailDto
{
    /// <summary>
    /// Truck identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Public URL slug for the truck.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Public listing title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Truck brand.
    /// </summary>
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    /// Truck model.
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Model year.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Truck mileage.
    /// </summary>
    public int Mileage { get; set; }

    /// <summary>
    /// Published price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Price currency code.
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the published price includes VAT.
    /// </summary>
    public bool PriceIncludesVat { get; set; }

    /// <summary>
    /// Available payment options for the truck.
    /// </summary>
    public string? PaymentOptions { get; set; }

    /// <summary>
    /// Truck category or type.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Transmission data when available.
    /// </summary>
    public string? Transmission { get; set; }

    /// <summary>
    /// Engine data when available.
    /// </summary>
    public string? Engine { get; set; }

    /// <summary>
    /// Configuration data when available.
    /// </summary>
    public string? Configuration { get; set; }

    /// <summary>
    /// State where the unit is located.
    /// </summary>
    public string LocationState { get; set; } = string.Empty;

    /// <summary>
    /// City where the unit is located, when available.
    /// </summary>
    public string? LocationCity { get; set; }

    /// <summary>
    /// Public description of the unit.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Public commercial status for the unit.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the unit should be promoted before other units.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Internal inventory number when available.
    /// </summary>
    public string? InternalNumber { get; set; }

    /// <summary>
    /// Vehicle identification or serial number when available.
    /// </summary>
    public string? VinOrSerial { get; set; }

    /// <summary>
    /// Unit color when available.
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Documentation status when available.
    /// </summary>
    public string? DocumentationStatus { get; set; }

    /// <summary>
    /// Mechanical condition when available.
    /// </summary>
    public string? MechanicalCondition { get; set; }

    /// <summary>
    /// Commercial observations when available.
    /// </summary>
    public string? CommercialObservations { get; set; }

    /// <summary>
    /// Ordered public truck images.
    /// </summary>
    public IReadOnlyCollection<TruckImageDto> Images { get; set; } = Array.Empty<TruckImageDto>();

    /// <summary>
    /// Related trucks for the detail page.
    /// </summary>
    public IReadOnlyCollection<RelatedTruckDto> RelatedTrucks { get; set; } = Array.Empty<RelatedTruckDto>();

    /// <summary>
    /// Creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp, when available.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
