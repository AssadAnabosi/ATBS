using System.Globalization;

namespace ATBS.Utils;

public static class ParseHelper
{
    public static Guid ParseGuid(string value, string field) =>
        Guid.TryParse(value.Trim(), out var result)
            ? result
            : throw new FormatException($"{field} '{value}' is not a valid GUID.");

    public static float ParseFloat(string value, string field) =>
        float.TryParse(value.Trim(), out var result)
            ? result
            : throw new FormatException($"{field} '{value}' is not a valid number.");

    public static DateTime ParseDate(string value, string field) =>
        DateTime.TryParse(value.Trim(), out var result)
            ? result
            : throw new FormatException($"{field} '{value}' is not a valid date.");
}