namespace ATBS.Utils;

public record CsvRow(int LineNumber, string[] Fields);

public class CsvHelper
{
    public static async Task<List<CsvRow>> ReadRowsAsync(string path)
    {
        if (!File.Exists(path))
        {
            return [];
        }

        var lines = await File.ReadAllLinesAsync(path);

        // Index 0 is the header; emit 1-based line numbers for human-friendly error reports.
        return lines
            .Select((line, index) => (line, lineNumber: index + 1))
            .Skip(1)
            .Where(x => !string.IsNullOrWhiteSpace(x.line))
            .Select(x => new CsvRow(x.lineNumber, x.line.Split(',')))
            .ToList();
    }
}