public class Menu
{
    private static string[][] commands =
    [
        ["help", "afficher toute les commandes"],
        ["client user/all", "afficher toute les clients ou les infos d'un client"],
        ["car id/name/all", "afficher toute les infos d'une voiture"],
    ];
    private static int selectedIndex = 0; // 0 si rien n'est select

    private static void SendCmd(string cmd)
    {
        string[] arguments = cmd.Split(" ");
    }

    private static void Display(string text)
    {
        Console.WriteLine(text);
    }

    private static void DisplayMenu()
    {
        // Console.BackgroundColor = ConsoleColor.Blue;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("# MENU: ");
        int index = 1;
        foreach (var info in commands)
        {
            if (index == selectedIndex % commands.Length + 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   ({index}) {info[0]} : {info[1]}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"   ({index}) {info[0]} : {info[1]}");
            }

            index++;
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    //https://dotnetperls.com/console-color

    private static void InitInteraction()
    {
        while (true)
        {
            var pressedKey = Console.ReadKey(true).Key;

            switch (pressedKey)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = selectedIndex + 1;
                    DisplayMenu();
                    // code block
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = selectedIndex - 1;
                    DisplayMenu();

                    // code block
                    break;
                default:
                    // code block
                    break;
            }
        }
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
