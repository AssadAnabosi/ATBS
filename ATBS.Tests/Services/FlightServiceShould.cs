using ATBS.Models;
using ATBS.Repositories;
using ATBS.Services;
using ATBS.Utils;
using Moq;

namespace ATBS.Tests.Services;

public class FlightServiceShould
{
    [Fact]
    public async Task ImportFromCsvAsync_ImportsValidRowsAndCollectsErrors()
    {
        var flights = new Mock<IFlightRepository>();
        var validation = new Mock<IValidationService>();
        var service = new FlightService(flights.Object, validation.Object);
        var rows = new List<CsvRow>
        {
            new(2, new[]
            {
                "1", "Egypt", "CAI", "2026-07-20", "France", "CDG", "2026-07-21", "100", "250", "500"
            }),
            new(3, new[] { "bad", "row" }),
            new(4, new[]
            {
                "2", "Egypt", "CAI", "2026-07-22", "France", "CDG", "2026-07-23", "120", "300", "600"
            })
        };

        flights.Setup(repository => repository.ReadRawRowsAsync("input.csv")).ReturnsAsync(rows);
        validation.Setup(serviceMock => serviceMock.Validate(It.IsAny<Flight>()))
            .Returns<Flight>(flight => flight.FlightId == 1
                ? []
                : new List<string> { "Rejected by validation" });

        var result = await service.ImportFromCsvAsync("input.csv");

        Assert.Single(result.ImportedFlights);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal(1, result.ImportedFlights[0].FlightId);
        Assert.Equal(3, result.Errors[0].LineNumber);
        Assert.Equal(4, result.Errors[1].LineNumber);

        flights.Verify(repository => repository.AddRangeAsync(It.Is<IEnumerable<Flight>>(items => items.Count() == 1)), Times.Once);
    }

    [Fact]
    public async Task ImportFromCsvAsync_DoesNotCallAddRangeWhenNoFlightsPass()
    {
        var flights = new Mock<IFlightRepository>();
        var validation = new Mock<IValidationService>();
        var service = new FlightService(flights.Object, validation.Object);
        var rows = new List<CsvRow>
        {
            new(1, new[] { "bad", "row" })
        };

        flights.Setup(repository => repository.ReadRawRowsAsync("input.csv")).ReturnsAsync(rows);

        var result = await service.ImportFromCsvAsync("input.csv");

        Assert.Empty(result.ImportedFlights);
        Assert.Single(result.Errors);
        flights.Verify(repository => repository.AddRangeAsync(It.IsAny<IEnumerable<Flight>>()), Times.Never);
    }

    [Fact]
    public async Task SearchAsync_FiltersCaseInsensitiveAndSortsByDepartureDate()
    {
        var flights = new Mock<IFlightRepository>();
        var validation = new Mock<IValidationService>();
        var service = new FlightService(flights.Object, validation.Object);
        var flightA = CreateFlight(1, new DateTime(2026, 7, 21, 15, 0, 0), "Egypt", "CAI", "France", "CDG", 200f, 300f, 400f);
        var flightB = CreateFlight(2, new DateTime(2026, 7, 20, 8, 0, 0), "EGYPT", "cai", "FRANCE", "cdg", 220f, 320f, 420f);
        var flightC = CreateFlight(3, new DateTime(2026, 7, 19, 8, 0, 0), "Egypt", "CAI", "France", "CDG", 220f, 320f, 420f);

        flights.Setup(repository => repository.GetAllAsync()).ReturnsAsync(new List<Flight> { flightA, flightB, flightC });

        var criteria = new FlightSearchCriteria
        {
            DepartureCountry = "egypt",
            DestinationCountry = "france",
            DepartureAirport = "cai",
            ArrivalAirport = "cdg",
            DepartureDate = new DateTime(2026, 7, 20),
            Class = CabinClass.Economy,
            MaxPrice = 300
        };

        var result = await service.SearchAsync(criteria);

        Assert.Single(result);
        Assert.Equal(2, result[0].FlightId);
    }

    [Fact]
    public async Task SearchAsync_SortsMatchingFlightsByDepartureDate()
    {
        var flights = new Mock<IFlightRepository>();
        var validation = new Mock<IValidationService>();
        var service = new FlightService(flights.Object, validation.Object);
        var flightA = CreateFlight(1, new DateTime(2026, 7, 22, 15, 0, 0), "Egypt", "CAI", "France", "CDG", 200f, 300f, 400f);
        var flightB = CreateFlight(2, new DateTime(2026, 7, 20, 8, 0, 0), "Egypt", "CAI", "France", "CDG", 220f, 320f, 420f);
        var flightC = CreateFlight(3, new DateTime(2026, 7, 21, 8, 0, 0), "Egypt", "CAI", "France", "CDG", 230f, 330f, 430f);

        flights.Setup(repository => repository.GetAllAsync()).ReturnsAsync(new List<Flight> { flightA, flightB, flightC });

        var result = await service.SearchAsync(new FlightSearchCriteria { DepartureCountry = "egypt" });

        Assert.Equal(new[] { 2, 3, 1 }, result.Select(flight => flight.FlightId));
    }

    [Fact]
    public void GetFlightConstraints_ReturnsValidationDescriptions()
    {
        var flights = new Mock<IFlightRepository>();
        var validation = new Mock<IValidationService>();
        var service = new FlightService(flights.Object, validation.Object);
        var constraints = new List<FieldConstraint>
        {
            new()
            {
                FieldName = "DepartureCountry",
                Type = "String",
                Constraints = new List<string> { "Required" }
            }
        };

        validation.Setup(serviceMock => serviceMock.DescribeConstraints<Flight>()).Returns(constraints);

        var result = service.GetFlightConstraints();

        Assert.Same(constraints, result);
        validation.Verify(serviceMock => serviceMock.DescribeConstraints<Flight>(), Times.Once);
    }

    private static Flight CreateFlight(
        int flightId,
        DateTime departureDate,
        string departureCountry,
        string departureAirport,
        string destinationCountry,
        string arrivalAirport,
        float economy,
        float business,
        float first)
    {
        return new Flight
        {
            FlightId = flightId,
            DepartureCountry = departureCountry,
            DepartureAirport = departureAirport,
            DepartureDate = departureDate,
            DestinationCountry = destinationCountry,
            ArrivalAirport = arrivalAirport,
            ArrivalDate = departureDate.AddHours(2),
            CabinPrices = new Dictionary<CabinClass, float>
            {
                [CabinClass.Economy] = economy,
                [CabinClass.Business] = business,
                [CabinClass.First] = first
            }
        };
    }
}
