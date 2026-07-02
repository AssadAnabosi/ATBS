namespace ATBS.Utils;

public class Constants
{
    public static readonly string DataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");
    public static class Files
    {
        public static readonly string Flights = Path.Combine(DataDirectory, "flights.csv");
        public static readonly string Bookings = Path.Combine(DataDirectory, "bookings.csv");
        public static readonly string SampleFlights = Path.Combine(DataDirectory, "sample-flights.csv");
    }
    
    public static class Csv
    {
        public const char Delimiter = ',';

        // Column order is the single source of truth for both reading and writing.
        public const string FlightHeader =
            "FlightId,DepartureCountry,DepartureAirport,DepartureDate,DestinationCountry,ArrivalAirport,ArrivalDate,EconomyPrice,BusinessPrice,FirstPrice";

        public const string BookingHeader =
            "BookingId,FlightId,PassengerEmail,PassengerName,CabinClass,Price,BookingDate,Status";
        
    }
}