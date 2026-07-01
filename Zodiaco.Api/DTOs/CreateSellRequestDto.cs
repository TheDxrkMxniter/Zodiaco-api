using System.ComponentModel.DataAnnotations;

namespace Zodiaco.Api.DTOs;

/// <summary>
/// Request body for creating a public sell request.
/// </summary>
public sealed class CreateSellRequestDto : IValidatableObject
{
    /// <summary>
    /// Seller name.
    /// </summary>
    [Required]
    [StringLength(150)]
    public string? Name { get; set; }

    /// <summary>
    /// Seller phone number.
    /// </summary>
    [Required]
    [StringLength(30)]
    public string? Phone { get; set; }

    /// <summary>
    /// Seller email when available.
    /// </summary>
    [StringLength(256)]
    public string? Email { get; set; }

    /// <summary>
    /// Company name when available.
    /// </summary>
    [StringLength(150)]
    public string? Company { get; set; }

    /// <summary>
    /// Seller truck type when available.
    /// </summary>
    [StringLength(80)]
    public string? TruckType { get; set; }

    /// <summary>
    /// Truck brand.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Brand { get; set; }

    /// <summary>
    /// Truck model.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Model { get; set; }

    /// <summary>
    /// Truck year when available.
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Truck mileage when available.
    /// </summary>
    public int? Mileage { get; set; }

    /// <summary>
    /// State where the unit is located.
    /// </summary>
    [StringLength(100)]
    public string? LocationState { get; set; }

    /// <summary>
    /// Expected selling price when available.
    /// </summary>
    public decimal? ExpectedPrice { get; set; }

    /// <summary>
    /// Documentation status when available.
    /// </summary>
    [StringLength(120)]
    public string? DocumentationStatus { get; set; }

    /// <summary>
    /// Additional comments when available.
    /// </summary>
    [StringLength(2000)]
    public string? Comments { get; set; }

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

        if (string.IsNullOrWhiteSpace(Brand))
        {
            yield return new ValidationResult("The Brand field is required.", [nameof(Brand)]);
        }

        if (string.IsNullOrWhiteSpace(Model))
        {
            yield return new ValidationResult("The Model field is required.", [nameof(Model)]);
        }

        var normalizedEmail = NormalizeOptionalText(Email);
        if (normalizedEmail is not null && !new EmailAddressAttribute().IsValid(normalizedEmail))
        {
            yield return new ValidationResult("The Email field is not a valid e-mail address.", [nameof(Email)]);
        }

        if (Year.HasValue)
        {
            var maxAllowedYear = DateTime.UtcNow.Year + 1;
            if (Year.Value < 1900 || Year.Value > maxAllowedYear)
            {
                yield return new ValidationResult(
                    $"The Year field must be between 1900 and {maxAllowedYear}.",
                    [nameof(Year)]);
            }
        }

        if (Mileage.HasValue && Mileage.Value < 0)
        {
            yield return new ValidationResult("The Mileage field must be zero or greater.", [nameof(Mileage)]);
        }

        if (ExpectedPrice.HasValue && ExpectedPrice.Value < 0)
        {
            yield return new ValidationResult("The ExpectedPrice field must be zero or greater.", [nameof(ExpectedPrice)]);
        }
    }

    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}
