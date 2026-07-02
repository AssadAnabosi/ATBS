using ATBS.UI;
using ATBS.Services;
using ATBS.Repositories;

var flightRepository = new FlightRepository();
var validationService = new ValidationService();

var bookingRepository = new  BookingRepository();

var flightService = new FlightService(flightRepository,  validationService);
var bookingSerivce = new BookingService(bookingRepository, flightRepository);

var passengerMenu = new PassengerMenu(bookingSerivce, flightService);
var managerMenu = new ManagerMenu(flightService, bookingSerivce);

var mainMenu = new MainMenu(passengerMenu, managerMenu);

await mainMenu.RunAsync();