using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    public static void Main()
    {
        #region Config Builder
        // https://dotnetfullstackdev.medium.com/appsettings-in-net-core-the-game-changer-for-configurations-a994d842e34c
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // le dossier de l'exécutable
            .AddJsonFile("appsettings.json", optional: false) // plante si le fichier manque
            .Build();
        #endregion
        Console.WriteLine(config.GetConnectionString("Default"));
        Console.WriteLine(DateTimeUtils.ConvertToDateTime("17-12-2007").Day);
    }
}
