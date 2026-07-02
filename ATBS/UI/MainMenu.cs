using ATBS.Utils;

namespace ATBS.UI;

/// <summary>
/// Top-level role selection. Choosing a role opens that role's menu; logging out of a role returns
/// here. Quit is only offered at this screen and exits the application.
/// </summary>
public sealed class MainMenu
{
    private readonly PassengerMenu _passengerMenu;
    private readonly ManagerMenu _managerMenu;

    public MainMenu(PassengerMenu passengerMenu, ManagerMenu managerMenu)
    {
        _passengerMenu = passengerMenu;
        _managerMenu = managerMenu;
    }
    
    public async Task RunAsync()
    {
        Console.WriteLine("Airport Ticket Booking System");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Select your role ===");
            Console.WriteLine("1) Passenger");
            Console.WriteLine("2) Manager");
            Console.WriteLine("0) Quit");
            
            switch (ConsoleInput.ReadRequiredText("Choose an option: "))
            {
                case "1":
                    await _passengerMenu.RunAsync();
                    break;
                case "2":
                    await _managerMenu.RunAsync();
                    break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Unknown option.");
                    break;
            }
        }
    }
}