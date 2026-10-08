using System.ComponentModel.DataAnnotations;
using ATBS.Attributes;
using ATBS.Models;
using ATBS.Services;

namespace ATBS.Tests.Utils;

public class ValidationAndAttributeShould
{
    [Fact]
    public void Validate_ValidFlight_ReturnsNoErrors()
    {
        var service = new ValidationService();
        var flight = CreateValidFlight();

        var errors = service.Validate(flight);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_InvalidFlight_ReturnsErrors()
    {
        var service = new ValidationService();
        var flight = CreateValidFlight();
        flight.DepartureCountry = string.Empty;
        flight.CabinPrices[CabinClass.Business] = 0;

        var errors = service.Validate(flight);

        Assert.NotEmpty(errors);
        Assert.Contains(errors, error => error.Contains("DepartureCountry"));
        Assert.Contains(errors, error => error.Contains("Business"));
    }

    [Fact]
    public void DescribeConstraints_Flight_ReturnsOnlyConstrainedFields()
    {
        var service = new ValidationService();

        var constraints = service.DescribeConstraints<Flight>();

        Assert.Contains(constraints, constraint => constraint.FieldName == nameof(Flight.DepartureCountry) && constraint.Constraints.Contains("Required"));
        Assert.Contains(constraints, constraint => constraint.FieldName == nameof(Flight.DepartureDate) && constraint.Constraints.Contains("Allowed Range (today → future)"));
        Assert.Contains(constraints, constraint => constraint.FieldName == nameof(Flight.CabinPrices) && constraint.Constraints.Contains("Requires all Cabin Classes to be defined and have a defined price"));
        Assert.DoesNotContain(constraints, constraint => constraint.FieldName == nameof(Flight.FlightId));
    }

    [Fact]
    public void NotInThePast_TodayOrFuture_ReturnsTrue()
    {
        var attribute = new NotInThePastAttribute();

        Assert.True(attribute.IsValid(DateTime.Today));
        Assert.True(attribute.IsValid(DateTime.Today.AddDays(1)));
    }

    [Fact]
    public void NotInThePast_PastDate_ReturnsFalse()
    {
        var attribute = new NotInThePastAttribute();

        Assert.False(attribute.IsValid(DateTime.Today.AddDays(-1)));
    }

    [Fact]
    public void ValidCabinPrices_CompletePositiveDictionary_ReturnsSuccess()
    {
        var attribute = new ValidCabinPricesAttribute();
        var flight = CreateValidFlight();
        var context = new ValidationContext(flight);

        var result = attribute.GetValidationResult(flight.CabinPrices, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ValidCabinPrices_MissingCabin_ReturnsError()
    {
        var attribute = new ValidCabinPricesAttribute();
        var flight = CreateValidFlight();
        flight.CabinPrices.Remove(CabinClass.First);
        var context = new ValidationContext(flight);

        var result = attribute.GetValidationResult(flight.CabinPrices, context);

        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Equal("First price is missing.", result?.ErrorMessage);
    }

    [Fact]
    public void ValidCabinPrices_NonPositivePrice_ReturnsError()
    {
        var attribute = new ValidCabinPricesAttribute();
        var flight = CreateValidFlight();
        flight.CabinPrices[CabinClass.Business] = 0;
        var context = new ValidationContext(flight);

        var result = attribute.GetValidationResult(flight.CabinPrices, context);

        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Equal("Business price must be greater than 0.", result?.ErrorMessage);
    }

    private static Flight CreateValidFlight() => new()
    {
        FlightId = 1,
        DepartureCountry = "Egypt",
        DepartureAirport = "CAI",
        DepartureDate = DateTime.Today.AddDays(1),
        DestinationCountry = "France",
        ArrivalAirport = "CDG",
        ArrivalDate = DateTime.Today.AddDays(2),
        CabinPrices = new Dictionary<CabinClass, float>
        {
            [CabinClass.Economy] = 100,
            [CabinClass.Business] = 200,
            [CabinClass.First] = 300
        }
    };
}
