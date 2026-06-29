using Microsoft.AspNetCore.Mvc;

namespace Zodiaco.Api.DTOs;

/// <summary>
/// Query string filters for the public truck catalog endpoint.
/// </summary>
public sealed class TruckListQueryDto
{
    /// <summary>
    /// Requested page number. Values lower than 1 are normalized to 1.
    /// </summary>
    [FromQuery(Name = "page")]
    public int? Page { get; set; }

    /// <summary>
    /// Requested page size. Values outside the 1 to 50 range are normalized to 12.
    /// </summary>
    [FromQuery(Name = "pageSize")]
    public int? PageSize { get; set; }

    /// <summary>
    /// Exact truck type filter.
    /// </summary>
    [FromQuery(Name = "type")]
    public string? Type { get; set; }

    /// <summary>
    /// Exact truck brand filter.
    /// </summary>
    [FromQuery(Name = "brand")]
    public string? Brand { get; set; }

    /// <summary>
    /// Minimum truck year, inclusive.
    /// </summary>
    [FromQuery(Name = "minYear")]
    public int? MinYear { get; set; }

    /// <summary>
    /// Maximum truck year, inclusive.
    /// </summary>
    [FromQuery(Name = "maxYear")]
    public int? MaxYear { get; set; }

    /// <summary>
    /// Exact state filter for the truck location.
    /// </summary>
    [FromQuery(Name = "locationState")]
    public string? LocationState { get; set; }

    /// <summary>
    /// Exact city filter for the truck location when a city exists.
    /// </summary>
    [FromQuery(Name = "locationCity")]
    public string? LocationCity { get; set; }

    /// <summary>
    /// Minimum price, inclusive.
    /// </summary>
    [FromQuery(Name = "minPrice")]
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Maximum price, inclusive.
    /// </summary>
    [FromQuery(Name = "maxPrice")]
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Text search across title, brand, model, type, location, and description.
    /// </summary>
    [FromQuery(Name = "search")]
    public string? Search { get; set; }
}
