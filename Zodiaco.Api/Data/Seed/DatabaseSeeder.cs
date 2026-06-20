using Microsoft.EntityFrameworkCore;
using Npgsql;
using Zodiaco.Api.Entities;
using Zodiaco.Api.Extensions;
using ZodiacoConfigurationExtensions = Zodiaco.Api.Extensions.ConfigurationExtensions;

namespace Zodiaco.Api.Data.Seed;

public sealed class DatabaseSeeder(AppDbContext dbContext, IConfiguration configuration)
{
    public async Task<int> SeedAsync(CancellationToken cancellationToken = default)
    {
        EnsureDevelopmentTargetsLocalDatabase();

        var seedItems = GetSeedItems();
        var normalizedSlugs = seedItems
            .Select(item => item.NormalizedSlug)
            .ToArray();

        var existingSlugs = await dbContext.Trucks
            .Where(truck => normalizedSlugs.Contains(truck.NormalizedSlug))
            .Select(truck => truck.NormalizedSlug)
            .ToListAsync(cancellationToken);

        var missingItems = seedItems
            .Where(item => !existingSlugs.Contains(item.NormalizedSlug, StringComparer.Ordinal))
            .Select(CreateTruck)
            .ToList();

        if (missingItems.Count == 0)
        {
            return 0;
        }

        await dbContext.Trucks.AddRangeAsync(missingItems, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return missingItems.Count;
    }

    private void EnsureDevelopmentTargetsLocalDatabase()
    {
        var connectionString = configuration.GetDatabaseConnectionString();
        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        if (!ZodiacoConfigurationExtensions.IsLocalHost(builder.Host))
        {
            throw new InvalidOperationException(
                "Seed is only allowed against a local PostgreSQL instance during development.");
        }
    }

    private static Truck CreateTruck(SeedTruck item)
    {
        return new Truck
        {
            Slug = item.Slug,
            NormalizedSlug = item.NormalizedSlug,
            Title = item.Title,
            Brand = item.Brand,
            Model = item.Model,
            Year = item.Year,
            Mileage = item.Mileage,
            Price = item.Price,
            Currency = item.Currency,
            Type = item.Type,
            Transmission = item.Transmission,
            Engine = item.Engine,
            Configuration = item.Configuration,
            LocationState = item.LocationState,
            LocationCity = item.LocationCity,
            Description = item.Description,
            Status = item.Status,
            IsFeatured = item.IsFeatured,
            IsPublished = item.IsPublished,
            CreatedAt = item.CreatedAt,
            Images = item.Images
                .Select((image, index) => new TruckImage
                {
                    Url = image.Url,
                    Alt = image.Alt,
                    SortOrder = index,
                    IsCover = index == 0,
                    CreatedAt = item.CreatedAt
                })
                .ToList()
        };
    }

    private static IReadOnlyList<SeedTruck> GetSeedItems()
    {
        var now = DateTime.UtcNow;

        return
        [
            new SeedTruck(
                "kenworth-t680-2020",
                "Kenworth T680 2020",
                "Kenworth",
                "T680",
                2020,
                485000,
                1845000m,
                "MXN",
                "Tractocamion",
                "Automatica",
                "PACCAR MX-13",
                "6x4 Sleeper",
                "Nuevo Leon",
                "Monterrey",
                "Tractocamion de largo recorrido con cabina amplia, historial de mantenimiento y configuracion lista para carretera.",
                "Available",
                true,
                true,
                now.AddDays(-21),
                [new SeedTruckImage("/images/trucks/kenworth-t680-2020-1.webp", "Kenworth T680 2020 frontal")]
            ),
            new SeedTruck(
                "freightliner-cascadia-2019",
                "Freightliner Cascadia 2019",
                "Freightliner",
                "Cascadia",
                2019,
                520000,
                1699000m,
                "MXN",
                "Tractocamion",
                "Automatica",
                "Detroit DD15",
                "6x4 Sleeper",
                "Jalisco",
                "Guadalajara",
                "Unidad pensada para flotas regionales con consumo optimizado y espacio suficiente para operador de ruta larga.",
                "Available",
                true,
                true,
                now.AddDays(-18),
                [new SeedTruckImage("/images/trucks/freightliner-cascadia-2019-1.webp", "Freightliner Cascadia 2019 lateral")]
            ),
            new SeedTruck(
                "international-prostar-2018",
                "International ProStar 2018",
                "International",
                "ProStar",
                2018,
                610000,
                1390000m,
                "MXN",
                "Tractocamion",
                "Manual",
                "Cummins ISX",
                "6x4 Sleeper",
                "Queretaro",
                "Queretaro",
                "Tractocamion con configuracion robusta para operaciones de carga general y costo competitivo para flotillas en crecimiento.",
                "Available",
                false,
                true,
                now.AddDays(-16),
                [new SeedTruckImage("/images/trucks/international-prostar-2018-1.webp", "International ProStar 2018 frente")]
            ),
            new SeedTruck(
                "kenworth-t370-rabon-2021",
                "Kenworth T370 Rabon 2021",
                "Kenworth",
                "T370",
                2021,
                185000,
                1540000m,
                "MXN",
                "Rabon",
                "Manual",
                "PACCAR PX-9",
                "4x2 Caja refrigerada",
                "Estado de Mexico",
                "Toluca",
                "Rabon para reparto interurbano con caja instalada, suspension reforzada y excelente maniobrabilidad para ciudad.",
                "Available",
                true,
                true,
                now.AddDays(-14),
                [new SeedTruckImage("/images/trucks/kenworth-t370-rabon-2021-1.webp", "Kenworth T370 Rabon 2021")]
            ),
            new SeedTruck(
                "freightliner-m2-torton-2020",
                "Freightliner M2 Torton 2020",
                "Freightliner",
                "M2",
                2020,
                244000,
                1625000m,
                "MXN",
                "Torton",
                "Manual",
                "Cummins ISB 6.7",
                "6x2 Caja seca",
                "Puebla",
                "Puebla",
                "Torton orientado a reparto pesado con configuracion de caja seca y ejes listos para trabajo comercial intensivo.",
                "Available",
                true,
                true,
                now.AddDays(-12),
                [new SeedTruckImage("/images/trucks/freightliner-m2-torton-2020-1.webp", "Freightliner M2 Torton 2020")]
            ),
            new SeedTruck(
                "mercedes-benz-boxer-autobus-2019",
                "Mercedes-Benz Boxer Autobus 2019",
                "Mercedes-Benz",
                "Boxer",
                2019,
                302000,
                1189000m,
                "MXN",
                "Autobus",
                "Manual",
                "OM 924 LA",
                "Pasaje urbano",
                "Hidalgo",
                "Pachuca",
                "Autobus para rutas urbanas o escolares con interior amplio, mantenimiento reciente y configuracion lista para servicio.",
                "Available",
                false,
                true,
                now.AddDays(-10),
                [new SeedTruckImage("/images/trucks/mercedes-benz-boxer-autobus-2019-1.webp", "Mercedes-Benz Boxer Autobus 2019")]
            ),
            new SeedTruck(
                "remolque-caja-seca-2022",
                "Remolque Caja Seca 2022",
                "Utility",
                "Caja Seca",
                2022,
                0,
                745000m,
                "MXN",
                "Caja seca",
                null,
                null,
                "53 pies",
                "San Luis Potosi",
                "San Luis Potosi",
                "Caja seca de 53 pies con estructura ligera y lista para operaciones de larga distancia o resguardo de mercancia.",
                "Available",
                false,
                true,
                now.AddDays(-8),
                [new SeedTruckImage("/images/trucks/remolque-caja-seca-2022-1.webp", "Remolque caja seca 2022")]
            ),
            new SeedTruck(
                "plataforma-40-pies-2021",
                "Plataforma 40 pies 2021",
                "Great Dane",
                "Plataforma 40 pies",
                2021,
                0,
                689000m,
                "MXN",
                "Plataforma",
                null,
                null,
                "40 pies",
                "Veracruz",
                "Veracruz",
                "Plataforma para carga abierta con piso reforzado, ideal para maquinaria, acero y materiales de gran volumen.",
                "Maintenance",
                false,
                false,
                now.AddDays(-6),
                [new SeedTruckImage("/images/trucks/plataforma-40-pies-2021-1.webp", "Plataforma 40 pies 2021")]
            )
        ];
    }

    private sealed record SeedTruck(
        string Slug,
        string Title,
        string Brand,
        string Model,
        int Year,
        int Mileage,
        decimal Price,
        string Currency,
        string Type,
        string? Transmission,
        string? Engine,
        string? Configuration,
        string LocationState,
        string? LocationCity,
        string Description,
        string Status,
        bool IsFeatured,
        bool IsPublished,
        DateTime CreatedAt,
        IReadOnlyList<SeedTruckImage> Images)
    {
        public string NormalizedSlug => Slug.Trim().ToLowerInvariant();
    }

    private sealed record SeedTruckImage(string Url, string Alt);
}
