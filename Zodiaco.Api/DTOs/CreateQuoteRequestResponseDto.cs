namespace Zodiaco.Api.DTOs;

/// <summary>
/// Response body for a created public quote request.
/// </summary>
public sealed class CreateQuoteRequestResponseDto
{
    /// <summary>
    /// Created quote request identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Saved quote request status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Confirmation message for the caller.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
