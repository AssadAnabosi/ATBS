using ATBS.Utils;
using ATBS.Models;

namespace ATBS.Repositories;
public class FlightRepository
{
    public Task<List<CsvRow>> ReadRawRowsAsync(string path) =>
        CsvHelper.ReadRowsAsync(path);
    
    public async Task<List<Flight>> GetAllAsync()
    {
        var rows = await CsvHelper.ReadRowsAsync(Constants.Files.Flights);
        return rows.Select(row => FlightCsv.FromRow(row.Fields)).ToList();
    }

    public async Task<Flight> GetByIdAsync(int flightId)
    {
        return (await GetAllAsync()).Where(f => f.FlightId == flightId).FirstOrDefault();
    }
    
    public async Task AddRangeAsync(IEnumerable<Flight> flights)
    {
        var all = (await GetAllAsync()).Concat(flights);
        await CsvHelper.WriteAsync(Constants.Files.Flights, Constants.Csv.FlightHeader, all, FlightCsv.ToRow);
    }
}