using System.Net;
using System.Text.RegularExpressions;

namespace Shannels;

internal static partial class Helper
{
    // AI: write comment explaining this regex to people that will read this code
    [GeneratedRegex(@"^(?!.*--)(?!-)(?!.*-$)[\w-]{3,20}$", RegexOptions.IgnoreCase)]
    public static partial Regex ValidUsername();

    public static (IPAddress, int port) ParseArgs(string[] args)
    {
        if (args is not [var ipString, var portString])
        {
            ShowUsageInfo("\nError: Please provide exactly two arguments: an IP address and a port number.");
            throw new ArgumentException("");
        }

        var validationErrors = new List<string>();

        if (!IPAddress.TryParse(ipString, out var ipAddress))
        {
            validationErrors.Add($"  - Invalid IP address format: '{ipString}'");
        }

        if (!int.TryParse(portString, out var port))
        {
            validationErrors.Add($"  - Port must be a valid number: '{portString}'");
        }
        else if (port is not > 1 and < 65535)
        {
            validationErrors.Add($"  - Port must be between 1 and 65535, but was: {port}");
        }

        if (validationErrors.Count != 0)
        {
            var errorMessage = "\nFound the following issues with your input:\n" + string.Join('\n', validationErrors);
            ShowUsageInfo(errorMessage);
            throw new ArgumentException("");
        }
        return (ipAddress!, port);
    }

    private static void ShowUsageInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.WriteLine("\nUsage: dotnet run <IP_ADDRESS> <PORT>");
        Console.WriteLine("Example: dotnet run 127.0.0.1 8080");
    }
}
