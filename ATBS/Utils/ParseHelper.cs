using System.Globalization;

namespace ATBS.Utils;

public static class ParseHelper
{
    public static int ParseInt(string value, string field) =>
        int.TryParse(value.Trim(), out var result)
            ? result
            : throw new FormatException($"{field} '{value}' is not a valid integer.");

    public static float ParseFloat(string value, string field) =>
        float.TryParse(value.Trim(), out var result)
            ? result
            : throw new FormatException($"{field} '{value}' is not a valid number.");

    public static DateTime ParseDate(string value, string field) =>
        DateTime.TryParse(value.Trim(), out var result)
            ? result
            : throw new FormatException($"{field} '{value}' is not a valid date.");
}