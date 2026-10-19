using Microsoft.Extensions.Configuration;

public class ConfigUtils
{
    public static IConfiguration GetConfig { get; } =
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // le dossier de l'exécutable
            .AddJsonFile("appsettings.json", optional: false) // plante si le fichier manque
            .AddEnvironmentVariables() // pour la chaîne de connexion
            .Build();
}
