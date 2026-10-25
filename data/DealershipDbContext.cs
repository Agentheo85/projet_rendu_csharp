using Dealership.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
public class DealershipDbContext : DbContext
{
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        // EF crée une table de liaison, ici nommée explicitement
        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.Cars)
            .WithMany(car => car.Owners)
            .UsingEntity(liaison => liaison.ToTable("CustomerCars")); // sert de table de liaison

        // le prix est stocké avec 2 décimales (10 chiffres au total)
        modelBuilder.Entity<Car>()
            .Property(car => car.Price)
            .HasPrecision(10, 2);
    }
    

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