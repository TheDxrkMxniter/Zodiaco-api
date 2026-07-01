using Zodiaco.Api.Common;
using Zodiaco.Api.Data;
using Zodiaco.Api.DTOs;
using Zodiaco.Api.Entities;

namespace Zodiaco.Api.Services;

public sealed class SellRequestsService(AppDbContext dbContext)
{
    private const string SellRequestCreatedMessage = "Solicitud para vender unidad registrada correctamente.";

    public async Task<CreateSellRequestResponseDto> CreateSellRequestAsync(
        CreateSellRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var sellRequest = new SellRequest
        {
            Name = NormalizeRequiredText(request.Name),
            Phone = NormalizeRequiredText(request.Phone),
            Email = NormalizeOptionalText(request.Email),
            Company = NormalizeOptionalText(request.Company),
            TruckType = NormalizeOptionalText(request.TruckType),
            Brand = NormalizeRequiredText(request.Brand),
            Model = NormalizeRequiredText(request.Model),
            Year = request.Year,
            Mileage = request.Mileage,
            LocationState = NormalizeOptionalText(request.LocationState),
            ExpectedPrice = request.ExpectedPrice,
            DocumentationStatus = NormalizeOptionalText(request.DocumentationStatus),
            Comments = NormalizeOptionalText(request.Comments),
            Status = LeadStatusValues.PendingReview
        };

        await dbContext.SellRequests.AddAsync(sellRequest, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateSellRequestResponseDto
        {
            Id = sellRequest.Id,
            Status = sellRequest.Status,
            Message = SellRequestCreatedMessage
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
