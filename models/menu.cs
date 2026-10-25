using Microsoft.VisualBasic;
using Spectre.Console;
using System.Globalization;
using Dealership;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dealership.Interfaces;
using Dealership.Models;
public class Menu
{
// - Voir l’ensemble des voitures 
// - Voir l’ensemble des clients 
// - Avoir plus d’information sur une voiture en particulier (ses caractéristiques, à qui elle appartient, …) 
// - Avoir plus d’information sur un client en particulier (ses informations, quelles voitures il possède, …) 
// - Vendre une voiture à un client que si elle n’est pas vendue 
// - Changer la voiture de client si elle est déjà vendue 
// - Combien vous avez gagné avec les voitures vendues dans la semaine
     private static Option[] options = new[]
    {
        new Option {Name= "GetAllCars", Descriptions = "Get all cars from the db" , Args = [] , CustomFunction = (args) => 
        {
  
              using var scope = DbUtils.host.Services.CreateScope();
            DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
            ICarRepository carsRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();
            var listCars = carsRepository.GetAllCars();
              var table = new Table();

            // Add columns https://spectreconsole.net/console/widgets/table/
          
  table.AddColumn("Brand");
            table.AddColumn("Model");
            table.AddColumn("Year");
            table.AddColumn("Color");
            table.AddColumn("Price");
            table.AddColumn("Sold");
            table.AddColumn("Sale date");
            foreach (var car in listCars)
            {
table.AddRow(car.Brand,car.Model,car.Year.ToString(),car.Color,car.Price.ToString()+" €",car.Sold ? "Oui" :"non",car.SaleDate?.ToString() ?? "");

            }
                        AnsiConsole.Write(table);

        }},
         new Option {Name= "GetAllCustomers", Descriptions = "Get all customers from the db" , Args = [] , CustomFunction = (args) => 
        {
  
              using var scope = DbUtils.host.Services.CreateScope();
            DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
            ICustomerRepository customersRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            var listCustomers = customersRepository.GetAllCustomer();
            var table = new Table();
              table.AddColumn("FirstName");
            table.AddColumn("LastName");
            table.AddColumn("birth");
            table.AddColumn("Age");
            table.AddColumn("Phone");
            table.AddColumn("Email");

            foreach (var customer in listCustomers)
            {
                table.AddRow(customer.FirstName,customer.LastName,customer.BirthDate.ToString(),customer.GetAge().ToString(),customer.Phone,customer.Email);

            }
            AnsiConsole.Write(table);
        }},
          new Option {Name= "Get Car Info", Descriptions = "Get Infos of the cars and the owners" , Args = ["CarName"] , CustomFunction = (args) => 
        {
  
              using var scope = DbUtils.host.Services.CreateScope();
             DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
            ICarRepository carsRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();
            Car? car = carsRepository.GetCarByName(args[0]);

            if (car is null)
            {
                AnsiConsole.MarkupLine("[red]Car not found[/]");
                return;
            }

            var table = new Table();

            // Add columns https://spectreconsole.net/console/widgets/table/
            table.AddColumn("Brand");
            table.AddColumn("Model");
            table.AddColumn("Year");
            table.AddColumn("Color");
            table.AddColumn("Price");
            table.AddColumn("Sold");
            table.AddColumn("Sale date");

            string saleDate = "-";
            if (car.SaleDate != null)
            {
                saleDate = car.SaleDate.Value.ToLocalTime().ToString();
            }

            string sold = "No";
            if (car.Sold)
            {
                sold = "Yes";
            }

            table.AddRow(car.Brand,car.Model,car.Year.ToString(),car.Color,car.Price.ToString()+" €",sold,saleDate);
            AnsiConsole.Write(table);

            var ownersTable = new Table();
            ownersTable.AddColumn("Owners");

            foreach (Customer owner in car.Owners)
            {
                ownersTable.AddRow(owner.GetFullName());
            }

            AnsiConsole.Write(ownersTable);
        
           
        }},
           new Option {Name= "Get Customer Info", Descriptions = "Get Infos of the customer and the cars he owns" , Args = ["FirstName","LastName"] , CustomFunction = (args) => 
        {
  
              using var scope = DbUtils.host.Services.CreateScope();
           DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
            ICustomerRepository customersRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            Customer? customer = customersRepository.GetCustomerByFirstandLastName(args[0],args[1]); // firstName , LastName

            if (customer is null)
            {
                AnsiConsole.MarkupLine("[red]Customer not found[/]");
                return;
            }

            var table = new Table();

            // Add columns https://spectreconsole.net/console/widgets/table/
            table.AddColumn("FirstName");
            table.AddColumn("LastName");
            table.AddColumn("birth");
            table.AddColumn("Age");
            table.AddColumn("Phone");
            table.AddColumn("Email");

            table.AddRow(customer.FirstName,customer.LastName,customer.BirthDate.ToString(),customer.GetAge().ToString(),customer.Phone,customer.Email);
            AnsiConsole.Write(table);

            var carsTable = new Table();
            carsTable.AddColumn("Cars");

            foreach (Car car in customer.Cars)
            {
                carsTable.AddRow(car.GetPresentableName());
            }

            AnsiConsole.Write(carsTable);
        
           
        }},
          new Option {Name= "Weekly Sales", Descriptions = "Get every cars solds in the weekend  and the total earnes" , Args = [] , CustomFunction = (args) =>
        {
// - Combien vous avez gagné avec les voitures vendues dans la semaine
              using var scope = DbUtils.host.Services.CreateScope();
            ICarRepository carsRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();
            List<Car> sales = carsRepository.GetWeeklySales();
            var table = new Table();

            table.AddColumn("Car");
            table.AddColumn("Customer");
            table.AddColumn("Date");
            table.AddColumn("Price");

            decimal total = 0;
            foreach (Car car in sales)
            {
                string owners = "";
                foreach (Customer owner in car.Owners)
                {
                    if (owners != "")
                    {
                        owners = owners + ", ";
                    }
                    owners = owners + owner.GetFullName();
                }

                string saleDate = "";
                if (car.SaleDate is not null)
                {
                    saleDate = car.SaleDate.ToString();
                }

                table.AddRow(car.GetPresentableName(),owners,saleDate,car.Price.ToString()+" €");
                total += car.Price;
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"Total : [green]{total} €[/]");

        }},
          new Option {Name= "Sell Car", Descriptions = "Sell car to the customer (only if is not aleardy sold)" , Args = ["CarName","FirstName","LastName"] , CustomFunction = (args) =>
        {
// - Vendre une voiture à un client que si elle n’est pas vendue
              using var scope = DbUtils.host.Services.CreateScope();
            ICarRepository carsRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();
            ICustomerRepository customersRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            Car? car = carsRepository.GetCarByName(args[0]);
            Customer? customer = customersRepository.GetCustomerByFirstandLastName(args[1],args[2]);
            int maxCar = 2;

            if (car is null)
            {
                AnsiConsole.MarkupLine("[red]Car not found[/]");
                return;
            }

            if (customer is null)
            {
                AnsiConsole.MarkupLine("[red]Customer not found[/]");
                return;
            }

            if (car.Sold)
            {
                AnsiConsole.MarkupLine("[red]Car already sold[/]");
                return;
            }

            if (customer.Cars.Count >= maxCar)
            {
                AnsiConsole.MarkupLine("[red]Customer already has 2 cars[/]");
                return;
            }

            car.Owners.Add(customer);
            car.Sold = true;
            car.SaleDate = DateTime.UtcNow;
            carsRepository.SaveChanges();

            AnsiConsole.MarkupLine($"[green]{car.GetPresentableName()} sold to {customer.GetFullName()}[/]");

        }},
          new Option {Name= "Change Car Owner", Descriptions = "Change the owner of a car" , Args = ["CarName","FirstName","LastName"] , CustomFunction = (args) =>
        {
// - Changer la voiture de client si elle est déjà vendue
              using var scope = DbUtils.host.Services.CreateScope();
            ICarRepository carsRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();
            ICustomerRepository customersRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            Car? car = carsRepository.GetCarByName(args[0]);
            Customer? customer = customersRepository.GetCustomerByFirstandLastName(args[1],args[2]);
            int maxCar = 2;

            if (car is null)
            {
                AnsiConsole.MarkupLine("[red]Car not found[/]");
                return;
            }

            if (customer is null)
            {
                AnsiConsole.MarkupLine("[red]Customer not found[/]");
                return;
            }

            if (!car.Sold)
            {
                AnsiConsole.MarkupLine("[red]Car is not sold[/]");
                return;
            }

            if (car.Owners.Contains(customer))
            {
                AnsiConsole.MarkupLine("[red]Customer already owns this car[/]");
                return;
            }

            if (customer.Cars.Count >= maxCar)
            {
                AnsiConsole.MarkupLine("[red]Customer already has 2 cars[/]");
                return;
            }

            car.Owners.Clear();
            car.Owners.Add(customer);
            carsRepository.SaveChanges();

            AnsiConsole.MarkupLine($"[green]{car.GetPresentableName()} owned now by {customer.GetFullName()}[/]");

        }},
          new Option {Name= "Reset Db", Descriptions = "Clear Db" , Args = [] , CustomFunction = (args) => 
        {
  
                  using var scope = DbUtils.host.Services.CreateScope();
             DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
           
AnsiConsole.Status()
        .Start("Reset de la DB", ctx =>
        {
        dbConnection.ClearDb();
        });
  
    AnsiConsole.MarkupLine("[red]Db Reset[/]");   

        }}
    };
    


 
    
    private static void DisplayMenu()
    {
        // https://spectreconsole.net/console/prompts/selection-prompt/#working-with-complex-objects
       var selectedOption = AnsiConsole.Prompt(
    new SelectionPrompt<Option>()
        .Title("Select a [green]Options[/]")
        .PageSize(15)
        .UseConverter(option => $"{option.Name} |  {option.Descriptions}")
        .AddChoices(options));

        AnsiConsole.MarkupLine($"You selected: [yellow]{selectedOption.Name}[/]");

        selectedOption.Start();
        AnsiConsole.WriteLine();
        DisplayMenu();
     
    }

    //https://dotnetperls.com/console-color

    private static void InitInteraction()
    {
       
    }

    public static void InitMenu(string prefixe)
    {
        DisplayMenu();
        InitInteraction();
    }
}

// Une fois l’initialisation faite, un menu en ligne de commande doit permettre de :
// - Voir l’ensemble des voitures
// - Voir l’ensemble des clients
// - Avoir plus d’information sur une voiture en particulier (ses caractéristiques, à qui elle appartient, …)
// - Avoir plus d’information sur un client en particulier (ses informations, quelles voitures il possède, …)
// - Vendre une voiture à un client que si elle n’est pas vendue
// - Changer la voiture de client si elle est déjà vendue
// - Combien vous avez gagné avec les voitures vendues dans la semaine
