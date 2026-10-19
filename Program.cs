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
        IConfiguration config = ConfigUtils.GetConfig;
        #endregion
        if (config.GetValue<bool>("debugOutput"))
        {
            Console.WriteLine(ParserUtils.ParseCsv("/", config["dataPath"])[1][1]);
            Console.WriteLine(config.GetConnectionString("Default"));
            Console.WriteLine(DateTimeUtils.ConvertToDateTime("17-12-2007").Day);
        }
    }
}
