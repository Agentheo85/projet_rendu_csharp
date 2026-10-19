using CoursSupDeVinci;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class SchoolDbContext : DbContext
{
    // --- Tables principales ---
    public DbSet<Classe> Classes { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Detail> Details { get; set; }

    // --- Constructeur ---
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
    {
    }
    
    // Constructeur vide pour EF CLI
    public SchoolDbContext() { }

    // --- Configuration des relations ---
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Classe>()
            .Property(c => c.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        
        modelBuilder.Entity<Person>()
            .Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        
        modelBuilder.Entity<Detail>()
            .Property(d => d.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        // Relation Classe -> Person (1..n)
        modelBuilder.Entity<Person>()
            .HasOne(p => p.Classe)
            .WithMany(c => c.Persons)
            .HasForeignKey(p => p.IdClasse);

        // Relation Detail -> Person (n..n)
        // Table d'association "person_detail"
        modelBuilder.Entity<Person>()
            .HasMany(p => p.Details)
            .WithMany(d => d.Persons)
            .UsingEntity(j =>
            {
                j.ToTable("person_detail");
                j.Property<Guid>("DetailsId").HasColumnName("id_detail");
                j.Property<Guid>("PersonsId").HasColumnName("id_person");
            });
    }

    // --- Configuration de la connexion ---
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Charger la configuration manuellement
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"/Users/antosox/Projects/CoursSupDeVinci/CoursSupDeVinci/appsettings.json", 
                optional: false, reloadOnChange: true)
            .Build();
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}