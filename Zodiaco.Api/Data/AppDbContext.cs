using Microsoft.EntityFrameworkCore;
using Zodiaco.Api.Entities;

namespace Zodiaco.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Truck> Trucks => Set<Truck>();
    public DbSet<TruckImage> TruckImages => Set<TruckImage>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<QuoteRequest> QuoteRequests => Set<QuoteRequest>();
    public DbSet<SellRequest> SellRequests => Set<SellRequest>();
    public DbSet<FinancingRequest> FinancingRequests => Set<FinancingRequest>();

    public override int SaveChanges()
    {
        NormalizeTrackedEntities();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        NormalizeTrackedEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        NormalizeTrackedEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        NormalizeTrackedEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(256);
            entity.Property(x => x.NormalizedEmail).IsRequired().HasMaxLength(256);
            entity.Property(x => x.PasswordHash).IsRequired().HasMaxLength(500);
            entity.Property(x => x.Role).IsRequired().HasMaxLength(50);
            entity.HasIndex(x => x.NormalizedEmail).IsUnique();
        });

        modelBuilder.Entity<Truck>(entity =>
        {
            entity.ToTable("Trucks");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Slug).IsRequired().HasMaxLength(180);
            entity.Property(x => x.NormalizedSlug).IsRequired().HasMaxLength(180);
            entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Brand).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Model).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Currency).IsRequired().HasMaxLength(3);
            entity.Property(x => x.Type).IsRequired().HasMaxLength(80);
            entity.Property(x => x.Transmission).HasMaxLength(120);
            entity.Property(x => x.Engine).HasMaxLength(120);
            entity.Property(x => x.Configuration).HasMaxLength(120);
            entity.Property(x => x.LocationState).IsRequired().HasMaxLength(100);
            entity.Property(x => x.LocationCity).HasMaxLength(100);
            entity.Property(x => x.Description).HasMaxLength(3000);
            entity.Property(x => x.Status).IsRequired().HasMaxLength(50);
            entity.HasIndex(x => x.NormalizedSlug).IsUnique();
            entity.HasIndex(x => x.Brand);
            entity.HasIndex(x => x.Type);
            entity.HasIndex(x => x.Status);
            entity.HasIndex(x => x.IsFeatured);
            entity.HasIndex(x => x.IsPublished);
            entity.HasIndex(x => x.LocationState);
            entity.Property(x => x.Price).HasPrecision(18, 2);

            entity.HasMany(x => x.Images)
                .WithOne(x => x.Truck)
                .HasForeignKey(x => x.TruckId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TruckImage>(entity =>
        {
            entity.ToTable("TruckImages");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Url).IsRequired().HasMaxLength(1000);
            entity.Property(x => x.Alt).HasMaxLength(200);
        });

        modelBuilder.Entity<Lead>(entity =>
        {
            entity.ToTable("Leads");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Phone).IsRequired().HasMaxLength(30);
            entity.Property(x => x.Email).HasMaxLength(256);
            entity.Property(x => x.Company).HasMaxLength(150);
            entity.Property(x => x.Source).HasMaxLength(100);
            entity.Property(x => x.Status).IsRequired().HasMaxLength(50);
            entity.Property(x => x.Message).HasMaxLength(2000);
            entity.HasIndex(x => x.Status);
        });

        modelBuilder.Entity<QuoteRequest>(entity =>
        {
            entity.ToTable("QuoteRequests");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Phone).IsRequired().HasMaxLength(30);
            entity.Property(x => x.Email).HasMaxLength(256);
            entity.Property(x => x.Company).HasMaxLength(150);
            entity.Property(x => x.Status).IsRequired().HasMaxLength(50);
            entity.Property(x => x.Message).HasMaxLength(2000);
            entity.HasIndex(x => x.Status);

            entity.HasOne(x => x.Truck)
                .WithMany()
                .HasForeignKey(x => x.TruckId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<SellRequest>(entity =>
        {
            entity.ToTable("SellRequests");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Phone).IsRequired().HasMaxLength(30);
            entity.Property(x => x.Email).HasMaxLength(256);
            entity.Property(x => x.Company).HasMaxLength(150);
            entity.Property(x => x.Brand).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Model).IsRequired().HasMaxLength(100);
            entity.Property(x => x.TruckType).HasMaxLength(80);
            entity.Property(x => x.LocationState).HasMaxLength(100);
            entity.Property(x => x.DocumentationStatus).HasMaxLength(100);
            entity.Property(x => x.Comments).HasMaxLength(2000);
            entity.Property(x => x.Status).IsRequired().HasMaxLength(50);
            entity.HasIndex(x => x.Status);
            entity.Property(x => x.ExpectedPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<FinancingRequest>(entity =>
        {
            entity.ToTable("FinancingRequests");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Phone).IsRequired().HasMaxLength(30);
            entity.Property(x => x.Email).HasMaxLength(256);
            entity.Property(x => x.Company).HasMaxLength(150);
            entity.Property(x => x.TruckType).HasMaxLength(80);
            entity.Property(x => x.Comments).HasMaxLength(2000);
            entity.Property(x => x.Status).IsRequired().HasMaxLength(50);
            entity.HasIndex(x => x.Status);
            entity.Property(x => x.UnitValue).HasPrecision(18, 2);
            entity.Property(x => x.DownPayment).HasPrecision(18, 2);
            entity.Property(x => x.EstimatedMonthlyPayment).HasPrecision(18, 2);
        });
    }

    private void NormalizeTrackedEntities()
    {
        ApplyAuditTimestamps();

        foreach (var entry in ChangeTracker.Entries<User>()
                     .Where(entry => entry.State is EntityState.Added or EntityState.Modified))
        {
            var trimmedEmail = entry.Entity.Email?.Trim() ?? string.Empty;
            entry.Entity.Email = trimmedEmail;
            entry.Entity.NormalizedEmail = string.IsNullOrEmpty(trimmedEmail)
                ? string.Empty
                : trimmedEmail.ToUpperInvariant();
        }

        foreach (var entry in ChangeTracker.Entries<Truck>()
                     .Where(entry => entry.State is EntityState.Added or EntityState.Modified))
        {
            var normalizedSlug = string.IsNullOrWhiteSpace(entry.Entity.Slug)
                ? string.Empty
                : entry.Entity.Slug.Trim().ToLowerInvariant();

            entry.Entity.Slug = normalizedSlug;
            entry.Entity.NormalizedSlug = normalizedSlug;
        }
    }

    private void ApplyAuditTimestamps()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries()
                     .Where(entry => entry.State is EntityState.Added or EntityState.Modified))
        {
            var createdAtProperty = entry.Metadata.FindProperty("CreatedAt");
            if (createdAtProperty?.ClrType == typeof(DateTime))
            {
                if (entry.State == EntityState.Added
                    && entry.Property("CreatedAt").CurrentValue is DateTime createdAt
                    && createdAt == default)
                {
                    entry.Property("CreatedAt").CurrentValue = utcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedAt").IsModified = false;
                }
            }

            var updatedAtProperty = entry.Metadata.FindProperty("UpdatedAt");
            if (updatedAtProperty is null)
            {
                continue;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = utcNow;
            }
        }
    }
}
