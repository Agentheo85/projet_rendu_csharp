using System.Globalization;
// using Dealership;
// using Dealership.Data;
// using Dealership.Interfaces;
// using Dealership.Models;
// using Dealership.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dealership.Models;

class Program
{
    static void InitDb()
    {
        IConfiguration configuration = ConfigUtils.GetConfig;

        #region csv
        string customerPath = Path.Combine(
            AppContext.BaseDirectory,
            configuration["dataPath"]!,
            "client.csv"
        ); // renvoie un path correcte
        string carPath = Path.Combine(
            AppContext.BaseDirectory,
            configuration["dataPath"]!,
            "voiture.csv"
        );
        #endregion

        #region read Customer
        List<string[]> customerBrutData = ParserUtils.ParseCsv("/", customerPath);
        List<Customer> customers = new List<Customer>();

        for (int i = 1; i < customerBrutData.Count; i++)
        {
            String[] data = customerBrutData[i];

            Customer customer = new Customer();

            customer.LastName = data[0];
            customer.FirstName = data[1];

            customer.BirthDate = DateTimeUtils.ConvertToDateTime(data[2]);

            customer.Phone = data[3];
            customer.Email = data[4];

            customers.Add(customer);
        }
        #endregion
        // voitures : Marque$Modele$Annee$Prix_HT$Couleur$Vendue

        List<string[]> carsBrutData = ParserUtils.ParseCsv("$", carPath);
        List<Car> cars = new List<Car>();

        for (int i = 1; i < carsBrutData.Count; i++)
        {
            String[] data = carsBrutData[i];

            Car car = new Car();

            car.Brand = data[0];
            car.Model = data[1];
            car.Year = Int32.Parse(data[2]);
            car.Price = Decimal.Parse(data[3], CultureInfo.InvariantCulture);
            car.Color = data[4];
            car.Sold = data[5].Trim().ToLower() == "oui";

            cars.Add(car);
        }
        customers.ForEach(customer => Console.WriteLine(customer.GetFullName()));
    }

    static void Main()
    {
        #region Config Builder
        // https://dotnetfullstackdev.medium.com/appsettings-in-net-core-the-game-changer-for-configurations-a994d842e34c
        IConfiguration configuration = ConfigUtils.GetConfig;
        #endregion
        if (configuration.GetValue<bool>("debugOutput"))
        {
            Console.WriteLine(
                ParserUtils.ParseCsv("/", configuration["dataPath"] + "client.csv")[1][1]
            );
            Console.WriteLine(configuration.GetConnectionString("Default"));
            Console.WriteLine(DateTimeUtils.ConvertToDateTime("17-12-2007").Day);
        }
        InitDb();
        Menu.InitMenu("-");
        // #region hôte et services
        // var host = Host.CreateDefaultBuilder()
        //     .ConfigureServices(services =>
        //     {
        //         // la chaîne de connexion à PostgreSQL vient de appsettings.json
        //         services.AddDbContext<DealershipDbContext>(options =>
        //             options.UseNpgsql(configuration.GetConnectionString("Default"))
        //         );

        //         services.AddTransient<DbConnection>();
        //         services.AddTransient<ICustomerRepository, CustomerRepository>();
        //         services.AddTransient<ICarRepository, CarRepository>();
        //         services.AddTransient<Menu>();
        //     })
        //     .Build();
        // #endregion

        // #region enregistrement en base
        // using var scope = host.Services.CreateScope();
        // DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
        // #endregion
    }
}
