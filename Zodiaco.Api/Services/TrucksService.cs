using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Zodiaco.Api.Common;
using Zodiaco.Api.Data;
using Zodiaco.Api.DTOs;

namespace Zodiaco.Api.Services;

public sealed class TrucksService(AppDbContext dbContext)
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 12;
    private const int MaxPageSize = 50;
    private const string LikeEscapeCharacter = "\\";

    public async Task<TruckListResponseDto> GetPublicCatalogAsync(
        TruckListQueryDto query,
        CancellationToken cancellationToken = default)
    {
        var page = query.Page.GetValueOrDefault(DefaultPage);
        if (page < 1)
        {
            page = DefaultPage;
        }

        var pageSize = NormalizePageSize(query.PageSize);

        var normalizedType = NormalizeFilterValue(query.Type);
        var normalizedBrand = NormalizeFilterValue(query.Brand);
        var normalizedLocationState = NormalizeFilterValue(query.LocationState);
        var normalizedLocationCity = NormalizeFilterValue(query.LocationCity);
        var normalizedSearch = NormalizeFilterValue(query.Search);

        IQueryable<Entities.Truck> trucksQuery = BuildPublicTrucksQuery();

        if (!string.IsNullOrWhiteSpace(normalizedType))
        {
            var typePattern = BuildExactLikePattern(normalizedType);
            trucksQuery = trucksQuery.Where(truck => EF.Functions.ILike(truck.Type, typePattern, LikeEscapeCharacter));
        }

        if (!string.IsNullOrWhiteSpace(normalizedBrand))
        {
            var brandPattern = BuildExactLikePattern(normalizedBrand);
            trucksQuery = trucksQuery.Where(truck => EF.Functions.ILike(truck.Brand, brandPattern, LikeEscapeCharacter));
        }

        if (query.MinYear.HasValue)
        {
            trucksQuery = trucksQuery.Where(truck => truck.Year >= query.MinYear.Value);
        }

        if (query.MaxYear.HasValue)
        {
            trucksQuery = trucksQuery.Where(truck => truck.Year <= query.MaxYear.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedLocationState))
        {
            var locationStatePattern = BuildExactLikePattern(normalizedLocationState);
            trucksQuery = trucksQuery.Where(truck => EF.Functions.ILike(truck.LocationState, locationStatePattern, LikeEscapeCharacter));
        }

        if (!string.IsNullOrWhiteSpace(normalizedLocationCity))
        {
            var locationCityPattern = BuildExactLikePattern(normalizedLocationCity);
            trucksQuery = trucksQuery.Where(truck =>
                truck.LocationCity != null
                && EF.Functions.ILike(truck.LocationCity, locationCityPattern, LikeEscapeCharacter));
        }

        if (query.MinPrice.HasValue)
        {
            trucksQuery = trucksQuery.Where(truck => truck.Price >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            trucksQuery = trucksQuery.Where(truck => truck.Price <= query.MaxPrice.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedSearch))
        {
            var searchPattern = BuildContainsLikePattern(normalizedSearch);

            trucksQuery = trucksQuery.Where(truck =>
                EF.Functions.ILike(truck.Title, searchPattern, LikeEscapeCharacter)
                || EF.Functions.ILike(truck.Brand, searchPattern, LikeEscapeCharacter)
                || EF.Functions.ILike(truck.Model, searchPattern, LikeEscapeCharacter)
                || EF.Functions.ILike(truck.Type, searchPattern, LikeEscapeCharacter)
                || EF.Functions.ILike(truck.LocationState, searchPattern, LikeEscapeCharacter)
                || (truck.LocationCity != null
                    && EF.Functions.ILike(truck.LocationCity, searchPattern, LikeEscapeCharacter))
                || (truck.Description != null
                    && EF.Functions.ILike(truck.Description, searchPattern, LikeEscapeCharacter)));
        }

        var totalItems = await trucksQuery.CountAsync(cancellationToken);
        var totalPages = totalItems == 0
            ? 0
            : (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = await trucksQuery
            .OrderByDescending(truck => truck.IsFeatured)
            .ThenByDescending(truck => truck.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(truck => new TruckListItemDto
            {
                Id = truck.Id,
                Slug = truck.Slug,
                Title = truck.Title,
                Brand = truck.Brand,
                Model = truck.Model,
                Year = truck.Year,
                Mileage = truck.Mileage,
                Price = truck.Price,
                Currency = truck.Currency,
                PriceIncludesVat = truck.PriceIncludesVat,
                PaymentOptions = truck.PaymentOptions,
                Type = truck.Type,
                LocationState = truck.LocationState,
                LocationCity = truck.LocationCity,
                Status = truck.Status,
                IsFeatured = truck.IsFeatured,
                CoverImageUrl = truck.Images
                    .OrderByDescending(image => image.IsCover)
                    .ThenBy(image => image.SortOrder)
                    .Select(image => image.Url)
                    .FirstOrDefault(),
                CoverImageAlt = truck.Images
                    .OrderByDescending(image => image.IsCover)
                    .ThenBy(image => image.SortOrder)
                    .Select(image => image.Alt)
                    .FirstOrDefault(),
                CreatedAt = truck.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new TruckListResponseDto
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }

    public async Task<TruckDetailDto?> GetPublicTruckDetailAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        var normalizedSlug = NormalizeSlug(slug);
        if (string.IsNullOrWhiteSpace(normalizedSlug))
        {
            return null;
        }

        var truck = await BuildPublicTrucksQuery()
            .Include(currentTruck => currentTruck.Images)
            .FirstOrDefaultAsync(currentTruck => currentTruck.NormalizedSlug == normalizedSlug, cancellationToken);

        if (truck is null)
        {
            return null;
        }

        var relatedTrucks = await BuildPublicTrucksQuery()
            .Where(relatedTruck => relatedTruck.Id != truck.Id)
            .OrderByDescending(relatedTruck => relatedTruck.Type == truck.Type)
            .ThenByDescending(relatedTruck => relatedTruck.IsFeatured)
            .ThenByDescending(relatedTruck => relatedTruck.CreatedAt)
            .Take(3)
            .Select(relatedTruck => new RelatedTruckDto
            {
                Id = relatedTruck.Id,
                Slug = relatedTruck.Slug,
                Title = relatedTruck.Title,
                Brand = relatedTruck.Brand,
                Model = relatedTruck.Model,
                Year = relatedTruck.Year,
                Price = relatedTruck.Price,
                Currency = relatedTruck.Currency,
                Type = relatedTruck.Type,
                LocationState = relatedTruck.LocationState,
                LocationCity = relatedTruck.LocationCity,
                CoverImageUrl = relatedTruck.Images
                    .OrderByDescending(image => image.IsCover)
                    .ThenBy(image => image.SortOrder)
                    .Select(image => image.Url)
                    .FirstOrDefault(),
                CoverImageAlt = relatedTruck.Images
                    .OrderByDescending(image => image.IsCover)
                    .ThenBy(image => image.SortOrder)
                    .Select(image => image.Alt)
                    .FirstOrDefault(),
                IsFeatured = relatedTruck.IsFeatured
            })
            .ToListAsync(cancellationToken);

        return new TruckDetailDto
        {
            Id = truck.Id,
            Slug = truck.Slug,
            Title = truck.Title,
            Brand = truck.Brand,
            Model = truck.Model,
            Year = truck.Year,
            Mileage = truck.Mileage,
            Price = truck.Price,
            Currency = truck.Currency,
            PriceIncludesVat = truck.PriceIncludesVat,
            PaymentOptions = truck.PaymentOptions,
            Type = truck.Type,
            Transmission = truck.Transmission,
            Engine = truck.Engine,
            Configuration = truck.Configuration,
            LocationState = truck.LocationState,
            LocationCity = truck.LocationCity,
            Description = truck.Description,
            Status = truck.Status,
            IsFeatured = truck.IsFeatured,
            InternalNumber = truck.InternalNumber,
            VinOrSerial = truck.VinOrSerial,
            Color = truck.Color,
            DocumentationStatus = truck.DocumentationStatus,
            MechanicalCondition = truck.MechanicalCondition,
            CommercialObservations = truck.CommercialObservations,
            Images = truck.Images
                .OrderBy(image => image.SortOrder)
                .ThenByDescending(image => image.IsCover)
                .Select(image => new TruckImageDto
                {
                    Id = image.Id,
                    Url = image.Url,
                    AltText = image.Alt,
                    SortOrder = image.SortOrder,
                    IsCover = image.IsCover
                })
                .ToList(),
            RelatedTrucks = relatedTrucks,
            CreatedAt = truck.CreatedAt,
            UpdatedAt = truck.UpdatedAt
        };
    }

    private IQueryable<Entities.Truck> BuildPublicTrucksQuery()
    {
        return dbContext.Trucks
            .AsNoTracking()
            .Where(truck => truck.IsPublished && truck.Status != TruckStatusValues.Hidden);
    }

    private static int NormalizePageSize(int? pageSize)
    {
        if (!pageSize.HasValue)
        {
            return DefaultPageSize;
        }

        if (pageSize.Value < 1 || pageSize.Value > MaxPageSize)
        {
            return DefaultPageSize;
        }

        return pageSize.Value;
    }

    private static string? NormalizeFilterValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static string NormalizeSlug(string slug)
    {
        return string.IsNullOrWhiteSpace(slug)
            ? string.Empty
            : slug.Trim().ToLowerInvariant();
    }

    private static string BuildExactLikePattern(string value)
    {
        return EscapeLikePattern(value);
    }

    private static string BuildContainsLikePattern(string value)
    {
        return $"%{EscapeLikePattern(value)}%";
    }

    private static string EscapeLikePattern(string value)
    {
        return value
            .Replace(LikeEscapeCharacter, $"{LikeEscapeCharacter}{LikeEscapeCharacter}", StringComparison.Ordinal)
            .Replace("%", $"{LikeEscapeCharacter}%", StringComparison.Ordinal)
            .Replace("_", $"{LikeEscapeCharacter}_", StringComparison.Ordinal);
    }
}
