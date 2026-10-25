using System.Globalization;
using Dealership;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using Dealership.Interfaces;
namespace Dealership.Models;


class Program
{ 
    static void InitDb()
    {
        IConfiguration configuration = ConfigUtils.GetConfig;
        using var scope = DbUtils.host.Services.CreateScope();
        DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();

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

        #region Seeder Sales Cars
        int maxCar = 2;
        Random random = new Random();

        for (int i = 0; i < cars.Count; i++)
        {
            Car car = cars[i];

            if (!car.Sold) continue;

            List<Customer> availableCustomers = customers.Where(customer => customer.Cars.Count < maxCar).ToList();

            if (availableCustomers.Count == 0)
            {
                car.Sold = false;
                continue;
            }

            Customer buyer = availableCustomers[random.Next(availableCustomers.Count)]; // on prend un client aleatoire dans la liste des clients avec moin de 2 vehicues et on lui donne une voiture
            car.Owners.Add(buyer);
            buyer.Cars.Add(car);
            car.SaleDate = DateTime.UtcNow;
        }
        #endregion
        dbConnection.InitDb(customers, cars);
        
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
        // doc AnsiConsole https://spectreconsole.net/console/
         AnsiConsole.MarkupLine("[bold blue]Projet[/] C# [green]Noann & Théo[/]");
  

  
    AnsiConsole.Status()
        .Start("Initiation de la Db...", ctx =>
        {
            InitDb();
        });
  
    AnsiConsole.MarkupLine("[green]Db Initié![/]");    
        Menu.InitMenu("-");
        #region hôte et services
      
        #endregion

  
        // #region enregistrement en base
        // using var scope = host.Services.CreateScope();
        // DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
        // #endregion
    }
}
