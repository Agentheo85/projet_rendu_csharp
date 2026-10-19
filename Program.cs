using System.Globalization;
using Dealership;
using Dealership.Data;
using Dealership.Interfaces;
using Dealership.Models;
using Dealership.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    static void Main(string[] args)
    {
        #region configuration
        // lecture de appsettings.json, avec rechargement automatique si le fichier change
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        #endregion

        #region hôte et services
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                // la chaîne de connexion à PostgreSQL vient de appsettings.json
                services.AddDbContext<DealershipDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("Default")));

                services.AddTransient<DbConnection>();
                services.AddTransient<ICustomerRepository, CustomerRepository>();
                services.AddTransient<ICarRepository, CarRepository>();
                services.AddTransient<Menu>();
            })
            .Build();
        #endregion

        #region csv
        // chemins des fichiers récupérés dans appsettings.json
        String customerPath = Path.Combine(AppContext.BaseDirectory, configuration["ClientCsvPath"]!);
        String carPath = Path.Combine(AppContext.BaseDirectory, configuration["VoitureCsvPath"]!);
        #endregion

        // clients : Nom@Prénom@Date de naissance@Téléphone@Email
        List<Customer> customers = new List<Customer>();

        String[] customerLines = File.ReadAllLines(customerPath);

        // i = 1 pour sauter la ligne d'en-tête
        for (int i = 1; i < customerLines.Length; i++)
        {
            String line = customerLines[i];
            if (String.IsNullOrWhiteSpace(line)) continue;

            Customer customer = new Customer();

            customer.LastName = line.Split('@')[0];
            customer.FirstName = line.Split('@')[1];

            // la date est lue au format jj/mm/aaaa, puis mise en UTC (PostgreSQL refuse les dates sans fuseau)
            DateTime birthDate = DateTime.ParseExact(line.Split('@')[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            customer.BirthDate = DateTime.SpecifyKind(birthDate, DateTimeKind.Utc);

            customer.Phone = line.Split('@')[3];
            customer.Email = line.Split('@')[4];

            customers.Add(customer);
        }

        // voitures : Marque$Modele$Annee$Prix_HT$Couleur$Vendue
        List<Car> cars = new List<Car>();

        String[] carLines = File.ReadAllLines(carPath);

        for (int i = 1; i < carLines.Length; i++)
        {
            String line = carLines[i];
            if (String.IsNullOrWhiteSpace(line)) continue;

            Car car = new Car();

            car.Brand = line.Split('$')[0];
            car.Model = line.Split('$')[1];
            car.Year = Int32.Parse(line.Split('$')[2]);
            car.PriceExcludingTax = Decimal.Parse(line.Split('$')[3], CultureInfo.InvariantCulture);
            car.Color = line.Split("$")[4];
            car.IsSold = line.Split("$")[5].Trim().ToLower() == "oui";

            cars.Add(car);
        }

        #region enregistrement en base
        using var scope = host.Services.CreateScope();
        DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
        #endregion
       
    }
}