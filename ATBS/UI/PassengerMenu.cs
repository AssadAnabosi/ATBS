using ATBS.Services;
using ATBS.Utils;
using ATBS.Models;

namespace ATBS.UI;

public sealed class PassengerMenu
{
    private readonly FlightService _flights;

    public PassengerMenu(FlightService flights)
    {
        _flights = flights;
    }

    public async Task RunAsync()
    {
        Console.WriteLine();
        Console.WriteLine("--- Passenger sign-in ---");
        Console.WriteLine("Your email: ");
        var email = Console.ReadLine();
        Console.WriteLine("Your name: ");
        var name = Console.ReadLine();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"=== Passenger menu ({name}) ===");
            Console.WriteLine("1) Search flights");
            Console.WriteLine("2) Book a flight");
            Console.WriteLine("3) View my bookings");
            Console.WriteLine("4) Modify a booking");
            Console.WriteLine("5) Cancel a booking");
            Console.WriteLine("0) Logout");
            Console.WriteLine("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await SearchFlightsAsync();
                    break;
                case "2":
                    Console.WriteLine("To Be Implemented");
                    break;
                case "3":
                    Console.WriteLine("To Be Implemented");
                    break;
                case "4":
                    Console.WriteLine("To Be Implemented");
                    break;
                case "5":
                    Console.WriteLine("To Be Implemented");
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Unknown option.");
                    break;
            }
        }
    }

    private async Task SearchFlightsAsync()
    {
        Console.WriteLine();
        Console.WriteLine("--- Search flights (leave blank to skip a filter) ---");
        var criteria = new FlightSearchCriteria
        {
            DepartureCountry = ConsoleInput.ReadOptionalText("Departure country: "),
            DestinationCountry = ConsoleInput.ReadOptionalText("Destination country: "),
            DepartureAirport = ConsoleInput.ReadOptionalText("Departure airport: "),
            ArrivalAirport = ConsoleInput.ReadOptionalText("Arrival airport: "),
            DepartureDate = ConsoleInput.ReadOptionalDate("Departure date (yyyy-MM-dd): "),
            Class = ConsoleInput.ReadOptionalClass("Class"),
            MaxPrice = ConsoleInput.ReadOptionalDecimal("Max price: ")
        };

        var results = await _flights.SearchAsync(criteria);

        if (results.Count == 0)
            Console.WriteLine("No flights found.");
        else
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
    }
}