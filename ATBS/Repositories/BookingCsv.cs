using ATBS.Models;
using ATBS.Utils;

namespace ATBS.Repositories;

public class BookingCsv
{
    private const int ExpectedColumns = 8;

    public static Booking FromRow(string[] fields)
    {
        if (fields.Length < ExpectedColumns)
        {
            throw new FormatException($"Expected {ExpectedColumns} columns but found {fields.Length}.");
        }

        return new Booking
        {
            BookingId = ParseHelper.ParseInt(fields[0], "BookingId"),
            FlightId = ParseHelper.ParseInt(fields[1], "FlightId"),
            PassengerEmail = fields[2].Trim(),
            PassengerName = fields[3].Trim(),
            CabinClass = Enum.TryParse<CabinClass>(fields[4].Trim(), ignoreCase: true, out var cabinClass)
                ? cabinClass
                : throw new FormatException($"CabinClass '{fields[4]}' is not valid."),
            Price = ParseHelper.ParseFloat(fields[5], "Price"),
            BookingDate = ParseHelper.ParseDate(fields[6], "BookingDate"),
            Status = Enum.TryParse<Status>(fields[7].Trim(), ignoreCase: true, out var status)
                ? status
                : throw new FormatException($"Status '{fields[7]}' is not valid.")
        };
    }

    public static string ToRow(Booking booking) =>
        string.Join(Constants.Csv.Delimiter, [
            booking.BookingId.ToString(),
            booking.FlightId.ToString(),
            booking.PassengerEmail,
            booking.PassengerName,
            booking.CabinClass.ToString(),
            booking.Price.ToString(),
            booking.BookingDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            booking.Status.ToString()
        ]);
}