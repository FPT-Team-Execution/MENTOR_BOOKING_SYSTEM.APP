using System.Globalization;

namespace MBS.Services.Utils;

public class ConvertUtils
{
    public static DateTime FormatDateTime(DateTime dateTime, string regex = "yyyy-MM-dd")
    {
        return DateTime.ParseExact(dateTime.ToString(regex), regex, CultureInfo.InvariantCulture);
    }
    public static string FormatDateTimeToString(DateTime? dateTime, string regex = "yyyy-MM-dd")
    {
       if(dateTime == null) return string.Empty;
        return dateTime.Value.ToString(regex);
    }
}