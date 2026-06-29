namespace Zodiaco.Api.DTOs;

/// <summary>
/// Public truck item returned to the frontend inventory listing.
/// </summary>
public sealed class TruckListItemDto
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
    /// Indicates whether the price includes VAT.
    /// </summary>
    public bool PriceIncludesVat { get; set; }

    /// <summary>
    /// Available payment options for the unit.
    /// </summary>
    public string? PaymentOptions { get; set; }

    /// <summary>
    /// Truck category or type.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// State where the unit is located.
    /// </summary>
    public string LocationState { get; set; } = string.Empty;

    /// <summary>
    /// City where the unit is located, when available.
    /// </summary>
    public string? LocationCity { get; set; }

    /// <summary>
    /// Public inventory status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the unit should be promoted before other units.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Cover image URL selected from the truck image collection.
    /// </summary>
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// Alternative text for the cover image, when available.
    /// </summary>
    public string? CoverImageAlt { get; set; }

    /// <summary>
    /// Creation timestamp used for public ordering.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
