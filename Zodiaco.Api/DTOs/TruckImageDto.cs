namespace Zodiaco.Api.DTOs;

/// <summary>
/// Public truck image returned in the truck detail endpoint.
/// </summary>
public sealed class TruckImageDto
{
    /// <summary>
    /// Image identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Public image URL.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Alternative text for the image.
    /// </summary>
    public string? AltText { get; set; }

    /// <summary>
    /// Sort order within the truck gallery.
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Indicates whether the image is marked as cover.
    /// </summary>
    public bool IsCover { get; set; }
}
