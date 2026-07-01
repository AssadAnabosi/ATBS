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
}