namespace ATBS.Models;

public sealed class RowError
{
    public required int LineNumber { get; init; }

    public required List<string> Errors { get; init; }

    public override string ToString()
    {
        return $"Line [{LineNumber}]: {string.Join("; ", Errors)}";
    }
}

public class ImportResult
{
    public List<Flight> ImportedFlights { get; set; }

    public List<RowError> Errors { get; set; }
}