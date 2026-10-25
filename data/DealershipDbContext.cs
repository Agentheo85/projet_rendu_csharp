using Dealership.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
public class DealershipDbContext : DbContext
{
 // --- Tables principales ---
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Car> Cars { get; set; }

       public DealershipDbContext(DbContextOptions<DealershipDbContext> options)
        : base(options)
    {
    }
    
    // Constructeur vide pour EF CLI
    public DealershipDbContext() { }


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
        var configuration = ConfigUtils.GetConfig;
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Default"));
        }
    }


}