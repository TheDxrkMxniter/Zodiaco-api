using Microsoft.EntityFrameworkCore;
using Zodiaco.Api.Common;
using Zodiaco.Api.Data;
using Zodiaco.Api.DTOs;
using Zodiaco.Api.Entities;

namespace Zodiaco.Api.Services;

public sealed class QuoteRequestsService(AppDbContext dbContext)
{
    private const string QuoteRequestCreatedMessage = "Solicitud de cotizacion registrada correctamente.";

    public async Task<CreateQuoteRequestResponseDto?> CreateQuoteRequestAsync(
        CreateQuoteRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var truckId = request.TruckId!.Value;

        var truckExists = await dbContext.Trucks
            .AsNoTracking()
            .AnyAsync(
                truck => truck.Id == truckId
                         && truck.IsPublished
                         && truck.Status != TruckStatusValues.Hidden,
                cancellationToken);

        if (!truckExists)
        {
            return null;
        }

        var quoteRequest = new QuoteRequest
        {
            TruckId = truckId,
            Name = NormalizeRequiredText(request.Name),
            Phone = NormalizeRequiredText(request.Phone),
            Email = NormalizeOptionalText(request.Email),
            Company = NormalizeOptionalText(request.Company),
            Message = NormalizeOptionalText(request.Message),
            Status = LeadStatusValues.PendingReview
        };

        await dbContext.QuoteRequests.AddAsync(quoteRequest, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateQuoteRequestResponseDto
        {
            Id = quoteRequest.Id,
            Status = quoteRequest.Status,
            Message = QuoteRequestCreatedMessage
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
