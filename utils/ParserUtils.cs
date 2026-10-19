public class ParserUtils
{
    public static List<String[]> ParseCsv(string separator, string filePath)
    {
        List<String[]> result = [];
        StreamReader reader = new StreamReader(File.OpenRead(filePath)); // on fait un reader qu'on va iterer
        while (!reader.EndOfStream) // des que on a tous iterer se sera la fin du stream donc la fin de la boucle.
        {
            String line = reader.ReadLine();
            String[] values = line.Split(separator);

            result.Add(values);
        }
        return result;
    }
}
