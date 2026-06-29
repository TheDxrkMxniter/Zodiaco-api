namespace Zodiaco.Api.DTOs;

/// <summary>
/// Paginated response for the public truck catalog endpoint.
/// </summary>
public sealed class TruckListResponseDto
{
    /// <summary>
    /// Current page items.
    /// </summary>
    public IReadOnlyCollection<TruckListItemDto> Items { get; set; } = Array.Empty<TruckListItemDto>();

    /// <summary>
    /// Resolved page number returned by the API.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Resolved page size returned by the API.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items that match the applied filters.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Total number of pages for the applied filters and page size.
    /// </summary>
    public int TotalPages { get; set; }
}
