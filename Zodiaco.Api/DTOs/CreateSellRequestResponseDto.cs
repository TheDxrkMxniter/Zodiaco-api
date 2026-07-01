namespace Zodiaco.Api.DTOs;

/// <summary>
/// Response body for a created public sell request.
/// </summary>
public sealed class CreateSellRequestResponseDto
{
    /// <summary>
    /// Created sell request identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Saved sell request status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Confirmation message for the caller.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
