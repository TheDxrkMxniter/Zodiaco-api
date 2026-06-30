namespace Zodiaco.Api.DTOs;

/// <summary>
/// Response body for a created public lead.
/// </summary>
public sealed class CreateLeadResponseDto
{
    /// <summary>
    /// Created lead identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Saved lead status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Confirmation message for the caller.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
