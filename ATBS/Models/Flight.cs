namespace ATBS.Models;

public enum CabinClass
{
    Economy,
    Business,
    First
}

public class Flight
{
    public Guid FlightId { get; set; }

    public string DepartureCountry { get; set; } = string.Empty;

    public string DepartureAirport { get; set; } = string.Empty;

    public DateTime DepartureDate { get; set; }

    public string DestinationCountry { get; set; } = string.Empty;

    public string ArrivalAirport { get; set; } = string.Empty;

    public DateTime ArrivalDate { get; set; }

    public required Dictionary<CabinClass, float> CabinPrices { get; set; }
}