using ATBS.UI;
using ATBS.Services;
using ATBS.Repositories;

var flightRepository = new FlightRepository();

var flightService = new FlightService(flightRepository);

var managerMenu = new ManagerMenu(flightService);

var mainMenu = new MainMenu(managerMenu);

await mainMenu.RunAsync();