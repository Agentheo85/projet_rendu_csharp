using System.Globalization;
using Dealership;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using Dealership.Interfaces;

public class DbUtils
{

      public static IHost host {get;} = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                // la chaîne de connexion à PostgreSQL vient de appsettings.json
                services.AddDbContext<DealershipDbContext>(options =>
                    options.UseNpgsql(ConfigUtils.GetConfig.GetConnectionString("Default"))
                );
        
                services.AddTransient<DbConnection>();
                services.AddTransient<ICustomerRepository, CustomerRepository>();
                services.AddTransient<ICarRepository, CarRepository>();
                services.AddTransient<Menu>();
            })
            .Build();
  
  
}