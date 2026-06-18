using Microsoft.EntityFrameworkCore;
using Vaede.Api.Entities;

namespace Vaede.Api.Data;

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
            entity.Property(x => x.Email).HasMaxLength(320);
            entity.Property(x => x.NormalizedEmail).HasMaxLength(320);
            entity.HasIndex(x => x.NormalizedEmail).IsUnique();
        });

        modelBuilder.Entity<Truck>(entity =>
        {
            entity.ToTable("Trucks");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Slug).HasMaxLength(200);
            entity.Property(x => x.NormalizedSlug).HasMaxLength(200);
            entity.HasIndex(x => x.NormalizedSlug).IsUnique();
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
        });

        modelBuilder.Entity<Lead>(entity =>
        {
            entity.ToTable("Leads");
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<QuoteRequest>(entity =>
        {
            entity.ToTable("QuoteRequests");
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Truck)
                .WithMany()
                .HasForeignKey(x => x.TruckId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<SellRequest>(entity =>
        {
            entity.ToTable("SellRequests");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ExpectedPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<FinancingRequest>(entity =>
        {
            entity.ToTable("FinancingRequests");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.UnitValue).HasPrecision(18, 2);
            entity.Property(x => x.DownPayment).HasPrecision(18, 2);
            entity.Property(x => x.EstimatedMonthlyPayment).HasPrecision(18, 2);
        });
    }

    private void NormalizeTrackedEntities()
    {
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
}
