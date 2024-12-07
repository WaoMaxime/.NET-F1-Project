using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.EF;

public class F1CarDbContext : DbContext
{
    public DbSet<F1Car> F1Cars { get; set; }
    public DbSet<FastestLap> FastestLaps { get; set; }
    public DbSet<Race> Races { get; set; }
    public DbSet<CarTyre> CarTyres { get; set; }

    public F1CarDbContext(DbContextOptions<F1CarDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        optionsBuilder.UseSqlite("Data Source=f1cars.db");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationships
        modelBuilder.Entity<FastestLap>()
            .HasOne(fl => fl.Car)
            .WithMany(fc => fc.FastestLaps)
            .HasForeignKey(fl => fl.CarId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FastestLap>()
            .HasOne(fl => fl.Race)
            .WithMany(r => r.FastestLaps)
            .HasForeignKey(fl => fl.RaceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CarTyre>()
            .HasOne(ct => ct.Race)
            .WithOne(r => r.Tyre)
            .HasForeignKey<CarTyre>(ct => ct.RaceId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CarTyre>()
            .HasKey(ct => new { ct.CarId, ct.Tyre });

        modelBuilder.Entity<CarTyre>()
            .Property(ct => ct.Tyre)
            .HasConversion<string>()
            .IsRequired();

        // Properties
        modelBuilder.Entity<Race>()
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Race>()
            .Property(r => r.Date)
            .IsRequired();

        modelBuilder.Entity<FastestLap>()
            .Property(fl => fl.Circuit)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<FastestLap>()
            .Property(fl => fl.LapTime)
            .IsRequired();

        modelBuilder.Entity<FastestLap>()
            .Property(fl => fl.DateOfRecord)
            .IsRequired();

        modelBuilder.Entity<F1Car>()
            .Property(fc => fc.Chasis)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<F1Car>()
            .Property(fc => fc.ManufactureDate)
            .IsRequired();

        modelBuilder.Entity<F1Car>()
            .Property(fc => fc.Team)
            .IsRequired();

        modelBuilder.Entity<F1Car>()
            .Property(fc => fc.Tyres)
            .HasConversion<string>()
            .IsRequired();
    }
    
    public bool CreateDatabase()
    {
        Console.WriteLine("Checking database...");
        var databaseCreated = Database.EnsureCreated();
        Console.WriteLine(databaseCreated ? "Database was created successfully." : "Database already exists.");
        return databaseCreated;
    }
    
    public void SeedDatabase()
    {
        Console.WriteLine("Seeding database...");
        if (F1Cars.Any() || Races.Any() || FastestLaps.Any() || CarTyres.Any())
        {
            Console.WriteLine("Database already contains data. Skipping seeding.");
            return;
        }
        
        DataSeeder.Seed(this);
        SaveChanges();
        Console.WriteLine("Database seeded successfully.");
    }
}
