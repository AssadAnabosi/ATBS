using ATBS.UI;
using ATBS.Services;
using ATBS.Repositories;

var flightRepository = new FlightRepository();
var validationService = new ValidationService();

var flightService = new FlightService(flightRepository,  validationService);

var passengerMenu = new PassengerMenu(flightService);
var managerMenu = new ManagerMenu(flightService);

var mainMenu = new MainMenu(passengerMenu, managerMenu);

await mainMenu.RunAsync();