using System.ComponentModel.DataAnnotations;

namespace Zodiaco.Api.DTOs;

/// <summary>
/// Request body for creating a public website lead.
/// </summary>
public sealed class CreateLeadRequestDto : IValidatableObject
{
    /// <summary>
    /// Contact name.
    /// </summary>
    [Required]
    [StringLength(150)]
    public string? Name { get; set; }

    /// <summary>
    /// Contact phone number.
    /// </summary>
    [Required]
    [StringLength(30)]
    public string? Phone { get; set; }

    /// <summary>
    /// Contact email when available.
    /// </summary>
    [StringLength(256)]
    public string? Email { get; set; }

    /// <summary>
    /// Company name when available.
    /// </summary>
    [StringLength(150)]
    public string? Company { get; set; }

    /// <summary>
    /// Optional lead message.
    /// </summary>
    [StringLength(2000)]
    public string? Message { get; set; }

    /// <summary>
    /// Lead source when available.
    /// </summary>
    [StringLength(100)]
    public string? Source { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            yield return new ValidationResult("The Name field is required.", [nameof(Name)]);
        }

        if (string.IsNullOrWhiteSpace(Phone))
        {
            yield return new ValidationResult("The Phone field is required.", [nameof(Phone)]);
        }

        var normalizedEmail = NormalizeOptionalText(Email);
        if (normalizedEmail is not null && !new EmailAddressAttribute().IsValid(normalizedEmail))
        {
            yield return new ValidationResult("The Email field is not a valid e-mail address.", [nameof(Email)]);
        }
    }

    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}
