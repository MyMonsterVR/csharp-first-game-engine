using System.Globalization;

namespace TestEngine;

public class Time
{
    public static string CurrentTime()
    {
        DateTime time = DateTime.Now;
        return time.ToString(CultureInfo.CurrentCulture);
    }
}