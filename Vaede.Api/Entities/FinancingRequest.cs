namespace Vaede.Api.Entities;

public class FinancingRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Company { get; set; }
    public string? TruckType { get; set; }
    public decimal? UnitValue { get; set; }
    public decimal? DownPayment { get; set; }
    public int? TermMonths { get; set; }
    public decimal? EstimatedMonthlyPayment { get; set; }
    public string? Comments { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
