namespace ATBS.UI;

public sealed class PassengerMenu
{
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
                    Console.WriteLine("To Be Implemented");
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
}