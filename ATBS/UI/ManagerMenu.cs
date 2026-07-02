using ATBS.Services;
using ATBS.Utils;

namespace ATBS.UI;

public sealed class ManagerMenu
{
    private FlightService _flights;
    
    public ManagerMenu(FlightService flights)
    {
        _flights = flights;
    }
    public async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Manager menu ===");
            Console.WriteLine("1) Import flights from CSV");
            Console.WriteLine("2) Filter bookings");
            Console.WriteLine("3) Show flight validation constraints");
            Console.WriteLine("4) View all flights");
            Console.WriteLine("0) Logout");
            
            switch (ConsoleInput.ReadRequiredText("Choose an option: "))
            {
                case "1":
                    await ImportFlightsAsync();
                    break;
                case "2":
                    Console.WriteLine("To Be Implemented");
                    break;
                case "3":
                    ShowConstraints();
                    break;
                case "4":
                    await ViewAllFlightsAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Unknown option.");
                    break;
            }
        }
    }

    private async Task ImportFlightsAsync()
    {
        string DataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");
        var path = Path.Combine(DataDirectory, "sample-flights.csv");

        if (!File.Exists(path))
        {
            Console.WriteLine($"File not found: {path}");
            return;
        }
        var result = await _flights.ImportFromCsvAsync(path);
        Console.WriteLine($"Imported {result.ImportedFlights.Count} flight(s); {result.Errors.Count} row(s) rejected.");

        foreach (var error in result.Errors)
        {
            Console.WriteLine($"\t{error}");
        }
    }
    
    private async Task ViewAllFlightsAsync()
    {
        var flights = await _flights.GetAllAsync();
        if (flights.Count == 0)
        {
            Console.WriteLine("No flights in the store yet. Import some first.");
            return;
        }

        Console.WriteLine($"{flights.Count} flight(s):");
        foreach (var flight in flights)
        {
            Console.WriteLine($"  {flight}");
        }
    }
    
    private void ShowConstraints()
    {
        Console.WriteLine();
        Console.WriteLine("--- Flight model validation constraints ---");
        foreach (var constraint in _flights.GetFlightConstraints())
        {
            Console.WriteLine($"  {constraint}");
        }
    }
}
