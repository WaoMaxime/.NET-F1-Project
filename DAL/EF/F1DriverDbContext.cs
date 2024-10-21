using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.EF
{
    public class F1CarDbContext : DbContext
    {
        public DbSet<F1Car> F1Cars { get; set; }
        public DbSet<FastestLap> FastestLaps { get; set; }
        
        public F1CarDbContext(DbContextOptions<F1CarDbContext> options)
            : base(options)
        {
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=hoofdentiteit.db");
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            }
        }
        
        public bool CreateDatabase(bool deleteIfExists)
        {
            if (deleteIfExists)
            {
                Console.WriteLine("Deleting existing database...");
                this.Database.EnsureDeleted(); 
            }

            Console.WriteLine("Creating database...");
            bool databaseCreated = this.Database.EnsureCreated();

            Console.WriteLine(databaseCreated ? "Database was created successfully." : "Database already exists.");
            
            return databaseCreated;
        }
    }
}