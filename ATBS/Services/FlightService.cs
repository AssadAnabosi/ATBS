using ATBS.Models;
using ATBS.Repositories;

namespace ATBS.Services;

public class FlightService
{
    private FlightRepository _flights;

    public FlightService(FlightRepository flights)
    {
        _flights = flights;
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

            imported.Add(flight);
        }

        return new ImportResult { ImportedFlights = imported, Errors = errors };
    }
}