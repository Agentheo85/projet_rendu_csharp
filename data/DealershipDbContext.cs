using Dealership.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration

namespace Dealership.Data;

public class DealershipDbContext : DbContext
{
    #region constructeur
    // les options (dont la connexion PostgreSQL) sont fournies par l'hôte
    public DealershipDbContext(DbContextOptions<DealershipDbContext> options) : base(options)
    {
    }
    #endregion

    #region tables
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Car> Cars { get; set; }
    #endregion

    #region configuration du modèle
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        // EF crée une table de liaison, ici nommée explicitement
        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.Cars)
            .WithMany(car => car.Customers)
            .UsingEntity(liaison => liaison.ToTable("CustomerCars"));

        // le prix est stocké avec 2 décimales (10 chiffres au total)
        modelBuilder.Entity<Car>()
            .Property(car => car.Price)
            .HasPrecision(10, 2);
    }
    #endregion

    // --- Configuration de la connexion ---
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Charger la configuration manuellement
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", 
                optional: false, reloadOnChange: true)
            .Build();
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Default"));
        }
    }
}
