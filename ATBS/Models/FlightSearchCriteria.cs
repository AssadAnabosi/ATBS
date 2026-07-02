namespace ATBS.Models;

public class FlightSearchCriteria
{
    public decimal? MaxPrice { get; init; }
    public string? DepartureCountry { get; init; }
    public string? DestinationCountry { get; init; }
    public DateTime? DepartureDate { get; init; }
    public string? DepartureAirport { get; init; }
    public string? ArrivalAirport { get; init; }
    public CabinClass? Class { get; init; }
}