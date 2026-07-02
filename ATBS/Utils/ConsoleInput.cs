using ATBS.Models;

namespace ATBS.Utils;

public class ConsoleInput
{
    public static int ReadRequiredInt(string prompt)
    {
        while (true)
        {
            var value = ReadRequiredText(prompt);
            if (int.TryParse(value, out var result))
            {
                return result;
            }

            Console.WriteLine("  Please enter a valid integer.");
        }
    }
    public static string ReadRequiredText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var value = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("  A value is required. Please try again.");
        }
    }

    public static string? ReadOptionalText(string prompt)
    {
        Console.Write(prompt);
        var value = Console.ReadLine();
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    public static decimal? ReadOptionalDecimal(string prompt)
    {
        var value = ReadOptionalText(prompt);
        if (value is null)
        {
            return null;
        }

        return decimal.TryParse(value, out var result)
            ? result
            : null;
    }

    public static DateTime? ReadOptionalDate(string prompt)
    {
        var value = ReadOptionalText(prompt);
        if (value is null)
        {
            return null;
        }

        return DateTime.TryParse(value, out var result)
            ? result
            : null;
    }

    public static CabinClass ReadRequiredClass(string prompt)
    {
        while (true)
        {
            var choice = ReadOptionalClass(prompt);
            if (choice is { } flightClass)
            {
                return flightClass;
            }

            Console.WriteLine("  Please choose a valid class number.");
        }
    }
    
    public static CabinClass? ReadOptionalClass(string prompt)
    {
        var value = ReadOptionalText($"{prompt} (0=Economy, 1=Business, 2=First Class): ");
        if (value is null)
        {
            return null;
        }

        return value switch
        {
            "0" => CabinClass.Economy,
            "1" => CabinClass.Business,
            "2" => CabinClass.First,
            _ => Enum.TryParse<CabinClass>(value, ignoreCase: true, out var parsed) ? parsed : null
        };
    }
}