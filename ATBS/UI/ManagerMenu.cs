namespace ATBS.UI;

public sealed class ManagerMenu
{
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
            Console.Write("Choose an option: ");
            
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
                case "0":
                    return;
                default:
                    Console.WriteLine("Unknown option.");
                    break;
            }
        }
    }
}
