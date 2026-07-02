using ATBS.Models;
using ATBS.Services;
using ATBS.Utils;

namespace ATBS.UI;

public sealed class ManagerMenu
{
    private readonly FlightService _flights;
    private readonly BookingService _bookings;

    public ManagerMenu(FlightService flights, BookingService bookings)
    {
        _flights = flights;
        _bookings = bookings;
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
                    await FilterBookingsAsync();
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

    private async Task FilterBookingsAsync()
    {
        Console.WriteLine();
        Console.WriteLine("--- Filter bookings (leave blank to skip a filter) ---");

        var flightIdText = ConsoleInput.ReadOptionalText("Flight ID: ");
        int? flightId = int.TryParse(flightIdText, out var fid) ? fid : null;
        var passengerEmail = ConsoleInput.ReadOptionalText("Passenger email: ");
        var passengerName = ConsoleInput.ReadOptionalText("Passenger name: ");
        var cabinClass = ConsoleInput.ReadOptionalClass("Cabin class");
        var maxPrice = ConsoleInput.ReadOptionalDecimal("Max price: ");
        var statusText = ConsoleInput.ReadOptionalText("Status (booked/cancelled): ");
        Status? status = Enum.TryParse<Status>(statusText, ignoreCase: true, out var s) ? s : null;

        var bookings = await _bookings.GetAllAsync();

        if (flightId.HasValue)
            bookings = bookings.Where(b => b.FlightId == flightId.Value).ToList();
        if (!string.IsNullOrEmpty(passengerEmail))
            bookings = bookings.Where(b => b.PassengerEmail.Equals(passengerEmail, StringComparison.OrdinalIgnoreCase))
                .ToList();
        if (!string.IsNullOrEmpty(passengerName))
            bookings = bookings.Where(b => b.PassengerName.Contains(passengerName, StringComparison.OrdinalIgnoreCase))
                .ToList();
        if (cabinClass.HasValue)
            bookings = bookings.Where(b => b.CabinClass == cabinClass.Value).ToList();
        if (maxPrice.HasValue)
            bookings = bookings.Where(b => b.Price <= (float)maxPrice.Value).ToList();
        if (status.HasValue)
            bookings = bookings.Where(b => b.Status == status.Value).ToList();

        if (bookings.Count == 0)
        {
            Console.WriteLine("No bookings match the criteria.");
            return;
        }

        var flightCache = new Dictionary<int, Flight?>();
        Console.WriteLine($"{bookings.Count} booking(s) found:");
        foreach (var booking in bookings)
        {
            if (!flightCache.TryGetValue(booking.FlightId, out var flight))
            {
                flight = await _flights.GetByIdAsync(booking.FlightId);
                flightCache[booking.FlightId] = flight;
            }

            Console.WriteLine($"  {booking}");
            if (flight is not null)
                Console.WriteLine($"    {flight}");
        }
    }
}