using Zodiaco.Api.Common;
using Zodiaco.Api.Data;
using Zodiaco.Api.DTOs;
using Zodiaco.Api.Entities;

namespace Zodiaco.Api.Services;

public sealed class LeadsService(AppDbContext dbContext)
{
    private const string DefaultLeadSource = "website";
    private const string LeadCreatedMessage = "Lead registrado correctamente.";

    public async Task<CreateLeadResponseDto> CreateLeadAsync(
        CreateLeadRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var lead = new Lead
        {
            Name = NormalizeRequiredText(request.Name),
            Phone = NormalizeRequiredText(request.Phone),
            Email = NormalizeOptionalText(request.Email),
            Company = NormalizeOptionalText(request.Company),
            Message = NormalizeOptionalText(request.Message),
            Source = NormalizeOptionalText(request.Source) ?? DefaultLeadSource,
            LeadType = LeadTypeValues.General,
            Status = LeadStatusValues.PendingReview
        };

        await dbContext.Leads.AddAsync(lead, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateLeadResponseDto
        {
            Id = lead.Id,
            Status = lead.Status,
            Message = LeadCreatedMessage
        };
    }

    private static string NormalizeRequiredText(string? value)
    {
        return value!.Trim();
    }

    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}
