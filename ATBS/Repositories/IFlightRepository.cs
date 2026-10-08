using ATBS.Models;
using ATBS.Utils;

namespace ATBS.Repositories;

public interface IFlightRepository
{
    Task<List<CsvRow>> ReadRawRowsAsync(string path);

    Task<List<Flight>> GetAllAsync();

    Task<Flight?> GetByIdAsync(int flightId);

    Task AddRangeAsync(IEnumerable<Flight> flights);
}
