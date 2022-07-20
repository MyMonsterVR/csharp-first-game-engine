namespace TestEngine;

public class Debug
{
    public static void Log(string s)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[DEBUG LOG]: {s}");
        Console.ResetColor();
    }
    
    public void Log(double s)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[DEBUG LOG]: {s}");
        Console.ResetColor();
    }
}