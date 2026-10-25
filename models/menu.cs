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
            var listCustomers = carsRepository.GetAllCars();
            foreach (var customers in listCustomers)
            {
                Console.WriteLine(customers.GetPresentableName());
            }
        }},
         new Option {Name= "GetAllCustomers", Descriptions = "Get all customers from the db" , Args = [] , CustomFunction = (args) => 
        {
  
              using var scope = DbUtils.host.Services.CreateScope();
            DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
            ICustomerRepository customersRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            var listCustomers = customersRepository.GetAllCustomer();
            foreach (var customers in listCustomers)
            {
                Console.WriteLine(customers.GetFullName());
            }
        }},
          new Option {Name= "Get Car Info", Descriptions = "Get Infos of the cars and the owners" , Args = ["CarName"] , CustomFunction = (args) => 
        {
  
              using var scope = DbUtils.host.Services.CreateScope();
             DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
            ICarRepository carsRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();
            Car? car = carsRepository.GetCarByName(args[0]);
            var table = new Table();
  
            // Add columns https://spectreconsole.net/console/widgets/table/
            table.AddColumn("Brand");
            table.AddColumn("Model");
            table.AddColumn("Year");
            table.AddColumn("Price");
            table.AddRow(car.Brand,car.Model,car.Year.ToString(),car.Price.ToString()+" €");
            AnsiConsole.Write(table);
        
           
        }},
           new Option {Name= "Get Customer Info", Descriptions = "Get Infos of the customer and the cars he owns" , Args = ["FirstName","LastName"] , CustomFunction = (args) => 
        {
  
              using var scope = DbUtils.host.Services.CreateScope();
           DbConnection dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();
            ICustomerRepository customersRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            Customer? customer = customersRepository.GetCustomerByFirstandLastName(args[0],args[1]);
            var table = new Table();
  
            // Add columns https://spectreconsole.net/console/widgets/table/
            table.AddColumn("FirstName");
            table.AddColumn("LastName");
            table.AddColumn("birth");
            table.AddColumn("Phone");
            table.AddColumn("Email");

            table.AddRow(customer.FirstName,customer.LastName,customer.BirthDate.ToString(),customer.Phone,customer.Email);
            AnsiConsole.Write(table);
        
           
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
    
    private static string[][] commands =
    [
        ["help", "afficher toute les commandes"],
        ["client user/all", "afficher toute les clients ou les infos d'un client"],
        ["car id/name/all", "afficher toute les infos d'une voiture"],
    ];

 
    private static void Display(string text)
    {
        Console.WriteLine(text);
    }

    private static void DisplayMenu()
    {
        // https://spectreconsole.net/console/prompts/selection-prompt/#working-with-complex-objects
       var selectedOption = AnsiConsole.Prompt(
    new SelectionPrompt<Option>()
        .Title("Select a [green]Options[/]")
        .PageSize(10)
        .UseConverter(option => $"{option.Name} |  {option.Descriptions})")
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
