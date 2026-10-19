using System.Globalization;

public class DateTimeUtils
{
    // https://www.c-sharpcorner.com/article/string-to-datetime-conversion-in-C-Sharp
    public static DateTime ConvertToDateTime(string? date)
    {
        if (
            DateTime.TryParseExact(
                date ?? "",
                "dd-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime birtdate
            )
        )
        {
            return DateTime.SpecifyKind(birtdate, DateTimeKind.Utc); // culture par default soit la culture anglaise
        }
        else
        {
            return DateTime.Now;
        }
    }
}
