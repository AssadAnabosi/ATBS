using ATBS.Models;
using ATBS.Repositories;

namespace ATBS.Tests.Repositories;

public class FlightCsvTests
{
    [Fact]
    public void FromRow_ValidFields_ReturnsFlight()
    {
        var fields = new[]
        {
            "5",
            "Egypt",
            "CAI",
            "2026-07-20",
            "France",
            "CDG",
            "2026-07-21",
            "120.5",
            "300",
            "600"
        };

        var flight = FlightCsv.FromRow(fields);

        Assert.Equal(5, flight.FlightId);
        Assert.Equal("Egypt", flight.DepartureCountry);
        Assert.Equal("CAI", flight.DepartureAirport);
        Assert.Equal(new DateTime(2026, 7, 20), flight.DepartureDate.Date);
        Assert.Equal("France", flight.DestinationCountry);
        Assert.Equal("CDG", flight.ArrivalAirport);
        Assert.Equal(new DateTime(2026, 7, 21), flight.ArrivalDate.Date);
        Assert.Equal(120.5f, flight.CabinPrices[CabinClass.Economy]);
        Assert.Equal(300f, flight.CabinPrices[CabinClass.Business]);
        Assert.Equal(600f, flight.CabinPrices[CabinClass.First]);
    }

    [Fact]
    public void FromRow_WithTooFewFields_ThrowsFormatException()
    {
        var fields = new[] { "1", "Egypt" };

        var exception = Assert.Throws<FormatException>(() => FlightCsv.FromRow(fields));

        Assert.Equal("Expected 10 columns but found 2.", exception.Message);
    }

    [Fact]
    public void ToRow_ReturnsExpectedCsvFormat()
    {
        var flight = new Flight
        {
            FlightId = 8,
            DepartureCountry = "Egypt",
            DepartureAirport = "CAI",
            DepartureDate = new DateTime(2026, 7, 20, 12, 15, 0),
            DestinationCountry = "France",
            ArrivalAirport = "CDG",
            ArrivalDate = new DateTime(2026, 7, 21, 16, 45, 0),
            CabinPrices = new Dictionary<CabinClass, float>
            {
                [CabinClass.Economy] = 120.5f,
                [CabinClass.Business] = 300f,
                [CabinClass.First] = 600f
            }
        };

        var row = FlightCsv.ToRow(flight);

        Assert.Equal("8,Egypt,CAI,2026-07-20T12:15:00,France,CDG,2026-07-21T16:45:00,120.5,300,600", row);
    }
}
