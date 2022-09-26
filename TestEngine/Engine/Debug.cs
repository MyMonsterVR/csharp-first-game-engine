using System.Diagnostics.CodeAnalysis;

namespace TestEngine;

public class Debug
{
    private static string _lastMessage;
    private static int retryAttempt = 0;

    /// <summary>
    ///    Writes a message to the console.
    /// </summary>
    /// <param name="T">Parameter value to pass.</param>
    /// <returns>Returns T based on the passed value.</returns>
    /// <example>Debug.Log($"Player speed: {player.Speed}");</example>
    public static void Log<T>(T logThis)
    {
        if (_lastMessage == logThis.ToString())
        {
            retryAttempt++;
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[DEBUG LOG]: {logThis}");
        Console.ResetColor();
        _lastMessage = logThis.ToString();
        retryAttempt = 0;
    }

    public void Error<T>(T logThis)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[DEBUG ERROR]: {logThis}");
        Console.ResetColor();
    }
}