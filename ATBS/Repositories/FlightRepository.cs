using ATBS.Utils;

namespace ATBS.Repositories;
public class FlightRepository
{
    public Task<List<CsvRow>> ReadRawRowsAsync(string path) =>
        CsvHelper.ReadRowsAsync(path);
}