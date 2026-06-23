using Microsoft.EntityFrameworkCore;
using Npgsql;
using Zodiaco.Api.Common;
using Zodiaco.Api.Entities;
using Zodiaco.Api.Extensions;
using ZodiacoConfigurationExtensions = Zodiaco.Api.Extensions.ConfigurationExtensions;

namespace Zodiaco.Api.Data.Seed;

public sealed class DatabaseSeeder(AppDbContext dbContext, IConfiguration configuration)
{
    public async Task<SeedResult> SeedAsync(CancellationToken cancellationToken = default)
    {
        EnsureDevelopmentTargetsLocalDatabase();

        var seedItems = GetSeedItems();
        var normalizedSlugs = seedItems
            .Select(item => item.NormalizedSlug)
            .ToArray();

        var existingTrucks = await dbContext.Trucks
            .Include(truck => truck.Images)
            .Where(truck => normalizedSlugs.Contains(truck.NormalizedSlug))
            .ToDictionaryAsync(truck => truck.NormalizedSlug, StringComparer.Ordinal, cancellationToken);

        var insertedCount = 0;
        var updatedCount = 0;

        foreach (var item in seedItems)
        {
            if (!existingTrucks.TryGetValue(item.NormalizedSlug, out var existingTruck))
            {
                await dbContext.Trucks.AddAsync(CreateTruck(item), cancellationToken);
                insertedCount++;
                continue;
            }

            if (ApplySeedData(existingTruck, item))
            {
                updatedCount++;
            }
        }

        if (insertedCount == 0 && updatedCount == 0)
        {
            return new SeedResult(0, 0);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return new SeedResult(insertedCount, updatedCount);
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
            InternalNumber = item.InternalNumber,
            VinOrSerial = item.VinOrSerial,
            Color = item.Color,
            DocumentationStatus = item.DocumentationStatus,
            MechanicalCondition = item.MechanicalCondition,
            CommercialObservations = item.CommercialObservations,
            PriceIncludesVat = item.PriceIncludesVat,
            PaymentOptions = item.PaymentOptions,
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

    private static bool ApplySeedData(Truck truck, SeedTruck item)
    {
        var hasChanges = false;

        hasChanges |= SetIfDifferent(truck.Slug, item.Slug, value => truck.Slug = value);
        hasChanges |= SetIfDifferent(truck.NormalizedSlug, item.NormalizedSlug, value => truck.NormalizedSlug = value);
        hasChanges |= SetIfDifferent(truck.Title, item.Title, value => truck.Title = value);
        hasChanges |= SetIfDifferent(truck.Brand, item.Brand, value => truck.Brand = value);
        hasChanges |= SetIfDifferent(truck.Model, item.Model, value => truck.Model = value);
        hasChanges |= SetIfDifferent(truck.Year, item.Year, value => truck.Year = value);
        hasChanges |= SetIfDifferent(truck.Mileage, item.Mileage, value => truck.Mileage = value);
        hasChanges |= SetIfDifferent(truck.Price, item.Price, value => truck.Price = value);
        hasChanges |= SetIfDifferent(truck.Currency, item.Currency, value => truck.Currency = value);
        hasChanges |= SetIfDifferent(truck.Type, item.Type, value => truck.Type = value);
        hasChanges |= SetIfDifferent(truck.Transmission, item.Transmission, value => truck.Transmission = value);
        hasChanges |= SetIfDifferent(truck.Engine, item.Engine, value => truck.Engine = value);
        hasChanges |= SetIfDifferent(truck.Configuration, item.Configuration, value => truck.Configuration = value);
        hasChanges |= SetIfDifferent(truck.InternalNumber, item.InternalNumber, value => truck.InternalNumber = value);
        hasChanges |= SetIfDifferent(truck.VinOrSerial, item.VinOrSerial, value => truck.VinOrSerial = value);
        hasChanges |= SetIfDifferent(truck.Color, item.Color, value => truck.Color = value);
        hasChanges |= SetIfDifferent(truck.DocumentationStatus, item.DocumentationStatus, value => truck.DocumentationStatus = value);
        hasChanges |= SetIfDifferent(truck.MechanicalCondition, item.MechanicalCondition, value => truck.MechanicalCondition = value);
        hasChanges |= SetIfDifferent(truck.CommercialObservations, item.CommercialObservations, value => truck.CommercialObservations = value);
        hasChanges |= SetIfDifferent(truck.PriceIncludesVat, item.PriceIncludesVat, value => truck.PriceIncludesVat = value);
        hasChanges |= SetIfDifferent(truck.PaymentOptions, item.PaymentOptions, value => truck.PaymentOptions = value);
        hasChanges |= SetIfDifferent(truck.LocationState, item.LocationState, value => truck.LocationState = value);
        hasChanges |= SetIfDifferent(truck.LocationCity, item.LocationCity, value => truck.LocationCity = value);
        hasChanges |= SetIfDifferent(truck.Description, item.Description, value => truck.Description = value);
        hasChanges |= SetIfDifferent(truck.Status, item.Status, value => truck.Status = value);
        hasChanges |= SetIfDifferent(truck.IsFeatured, item.IsFeatured, value => truck.IsFeatured = value);
        hasChanges |= SetIfDifferent(truck.IsPublished, item.IsPublished, value => truck.IsPublished = value);
        if (ImagesDiffer(truck.Images, item.Images))
        {
            truck.Images.Clear();
            foreach (var image in item.Images.Select((image, index) => new TruckImage
                     {
                         Url = image.Url,
                         Alt = image.Alt,
                         SortOrder = index,
                         IsCover = index == 0,
                         CreatedAt = item.CreatedAt
                     }))
            {
                truck.Images.Add(image);
            }

            hasChanges = true;
        }

        return hasChanges;
    }

    private static bool ImagesDiffer(ICollection<TruckImage> currentImages, IReadOnlyList<SeedTruckImage> seedImages)
    {
        var orderedCurrentImages = currentImages
            .OrderBy(image => image.SortOrder)
            .ToArray();

        if (orderedCurrentImages.Length != seedImages.Count)
        {
            return true;
        }

        for (var index = 0; index < orderedCurrentImages.Length; index++)
        {
            var currentImage = orderedCurrentImages[index];
            var seedImage = seedImages[index];

            if (!string.Equals(currentImage.Url, seedImage.Url, StringComparison.Ordinal)
                || !string.Equals(currentImage.Alt, seedImage.Alt, StringComparison.Ordinal)
                || currentImage.SortOrder != index
                || currentImage.IsCover != (index == 0))
            {
                return true;
            }
        }

        return false;
    }

    private static bool SetIfDifferent<T>(T currentValue, T newValue, Action<T> assign)
        where T : IEquatable<T>?
    {
        if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
        {
            return false;
        }

        assign(newValue);
        return true;
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
                TruckCurrencyValues.Mxn,
                TruckCategoryValues.Tractocamion,
                "Automatica",
                "PACCAR MX-13",
                "6x4 Sleeper",
                "UZ-T680-2020-001",
                "3WKYD49X9LF123001",
                "Blanco",
                "Factura original y tenencias al corriente",
                "Mantenimiento preventivo reciente",
                "Unidad lista para entrega inmediata con historial verificado.",
                true,
                PaymentOptionValues.CashAndFinancing,
                "Nuevo Leon",
                "Monterrey",
                "Tractocamion de largo recorrido con cabina amplia, historial de mantenimiento y configuracion lista para carretera.",
                TruckStatusValues.Available,
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
                TruckCurrencyValues.Mxn,
                TruckCategoryValues.Tractocamion,
                "Automatica",
                "Detroit DD15",
                "6x4 Sleeper",
                "UZ-CASC-2019-002",
                "1FUJGLDR4KSK23002",
                "Azul",
                "Documentacion completa para emplacado",
                "Servicio mayor realizado en taller autorizado",
                "Precio competitivo para renovacion de flota.",
                true,
                PaymentOptionValues.CashAndFinancing,
                "Jalisco",
                "Guadalajara",
                "Unidad pensada para flotas regionales con consumo optimizado y espacio suficiente para operador de ruta larga.",
                TruckStatusValues.Available,
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
                TruckCurrencyValues.Mxn,
                TruckCategoryValues.Tractocamion,
                "Manual",
                "Cummins ISX",
                "6x4 Sleeper",
                "UZ-PROS-2018-003",
                "3HSDJAPR1JN43003",
                "Rojo",
                "Factura y baja disponibles",
                "Condicion mecanica estable para trabajo continuo",
                "Ideal para compradores que priorizan costo-beneficio.",
                true,
                PaymentOptionValues.CashAndFinancing,
                "Queretaro",
                "Queretaro",
                "Tractocamion con configuracion robusta para operaciones de carga general y costo competitivo para flotillas en crecimiento.",
                TruckStatusValues.Available,
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
                TruckCurrencyValues.Mxn,
                TruckCategoryValues.CamionRabon,
                "Manual",
                "PACCAR PX-9",
                "4x2 Caja refrigerada",
                "UZ-T370-2021-004",
                "2NKHHM6X5MM44004",
                "Blanco",
                "Documentacion lista para cesion",
                "Suspension y frenos revisados",
                "Unidad muy atractiva para reparto refrigerado.",
                true,
                PaymentOptionValues.CashAndFinancing,
                "Estado de Mexico",
                "Toluca",
                "Rabon para reparto interurbano con caja instalada, suspension reforzada y excelente maniobrabilidad para ciudad.",
                TruckStatusValues.Available,
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
                TruckCurrencyValues.Mxn,
                TruckCategoryValues.CamionTorton,
                "Manual",
                "Cummins ISB 6.7",
                "6x2 Caja seca",
                "UZ-M2-2020-005",
                "3ALACWFC7LD55005",
                "Blanco",
                "Papeles en regla y verificacion vigente",
                "Motor y caja en condiciones operativas",
                "Listo para integrarse a operaciones de reparto pesado.",
                true,
                PaymentOptionValues.CashAndFinancing,
                "Puebla",
                "Puebla",
                "Torton orientado a reparto pesado con configuracion de caja seca y ejes listos para trabajo comercial intensivo.",
                TruckStatusValues.Available,
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
                TruckCurrencyValues.Mxn,
                TruckCategoryValues.AutobusUrbano,
                "Manual",
                "OM 924 LA",
                "Pasaje urbano",
                "UZ-BOXER-2019-006",
                "9BM688277KB66006",
                "Amarillo",
                "Permisos listos para revision",
                "Operacion estable con mantenimiento documentado",
                "Buena opcion para rutas urbanas o escolares.",
                true,
                PaymentOptionValues.Financing,
                "Hidalgo",
                "Pachuca",
                "Autobus para rutas urbanas o escolares con interior amplio, mantenimiento reciente y configuracion lista para servicio.",
                TruckStatusValues.Available,
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
                TruckCurrencyValues.Mxn,
                TruckCategoryValues.RemolqueCajaSeca,
                null,
                null,
                "53 pies",
                "UZ-CS-2022-007",
                "1UYVS2537NU77007",
                "Blanco",
                "Documentacion corporativa completa",
                "Sin observaciones mecanicas relevantes",
                "Remolque de alta rotacion para operaciones logísticas.",
                true,
                PaymentOptionValues.Cash,
                "San Luis Potosi",
                "San Luis Potosi",
                "Caja seca de 53 pies con estructura ligera y lista para operaciones de larga distancia o resguardo de mercancia.",
                TruckStatusValues.Available,
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
                TruckCurrencyValues.Mxn,
                TruckCategoryValues.RemolquePlataforma,
                null,
                null,
                "40 pies",
                "UZ-PLAT-2021-008",
                "1GRAA0624MB88008",
                "Negro",
                "Expediente en revision interna",
                "Pendiente de acondicionamiento estetico",
                "Oculta para pruebas de inventario no publicado.",
                true,
                PaymentOptionValues.CashAndFinancing,
                "Veracruz",
                "Veracruz",
                "Plataforma para carga abierta con piso reforzado, ideal para maquinaria, acero y materiales de gran volumen.",
                TruckStatusValues.Hidden,
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
        string? InternalNumber,
        string? VinOrSerial,
        string? Color,
        string? DocumentationStatus,
        string? MechanicalCondition,
        string? CommercialObservations,
        bool PriceIncludesVat,
        string? PaymentOptions,
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

    public sealed record SeedResult(int InsertedCount, int UpdatedCount);
}
