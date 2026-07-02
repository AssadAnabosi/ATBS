using ATBS.Models;
using ATBS.Repositories;

namespace ATBS.Services;

public class FlightService
{
    private FlightRepository _flights;
    private ValidationService _validation;

    public FlightService(FlightRepository flights, ValidationService validation)
    {
        _flights = flights;
        _validation = validation;
    }

    public async Task<ImportResult> ImportFromCsvAsync(string path)
    {
        var rows = await _flights.ReadRawRowsAsync(path);

        var imported = new List<Flight>();
        var errors = new List<RowError>();

        foreach (var row in rows)
        {
            Flight flight;
            try
            {
                flight = FlightCsv.FromRow(row.Fields);
            }
            catch (FormatException ex)
            {
                errors.Add(new RowError { LineNumber = row.LineNumber, Errors = [ex.Message] });
                continue;
            }

            var validationErrors = _validation.Validate(flight);
            if (validationErrors.Count > 0)
            {
                errors.Add(new RowError { LineNumber = row.LineNumber, Errors = validationErrors });
                continue;
            }

            imported.Add(flight);
        }

        if (imported.Count > 0)
        {
            await _flights.AddRangeAsync(imported);
        }

        return new ImportResult { ImportedFlights = imported, Errors = errors };
    }

    public Task<List<Flight>> GetAllAsync() => _flights.GetAllAsync();

    public List<FieldConstraint> GetFlightConstraints() =>
        _validation.DescribeConstraints<Flight>();

    public async Task<List<Flight>> SearchAsync(FlightSearchCriteria criteria)
    {
        // Build one predicate per supplied criterion
        var predicates = BuildPredicates(criteria);
        var flights = await _flights.GetAllAsync();

        return flights
            .Where(flight => predicates.All(matches => matches(flight)))
            .OrderBy(flight => flight.DepartureDate)
            .ToList();
    }

    private List<Func<Flight, bool>> BuildPredicates(FlightSearchCriteria criteria)
    {
        var predicates = new List<Func<Flight, bool>>();

        if (criteria.MaxPrice.HasValue)
        {
            if (criteria.Class.HasValue)
                predicates.Add(flight => flight.CabinPrices[criteria.Class.Value] <= (float)criteria.MaxPrice.Value);
        }

        if (!string.IsNullOrEmpty(criteria.DepartureCountry))
            predicates.Add(flight => flight.DepartureCountry.Equals(criteria.DepartureCountry, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(criteria.DestinationCountry))
            predicates.Add(flight => flight.DestinationCountry.Equals(criteria.DestinationCountry, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(criteria.DepartureAirport))
            predicates.Add(flight => flight.DepartureAirport.Equals(criteria.DepartureAirport, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(criteria.ArrivalAirport))
            predicates.Add(flight => flight.ArrivalAirport.Equals(criteria.ArrivalAirport, StringComparison.OrdinalIgnoreCase));

        if (criteria.DepartureDate.HasValue)
            predicates.Add(flight => flight.DepartureDate.Date == criteria.DepartureDate.Value.Date);

        return predicates;
    }
}