using ATBS.Models;
using ATBS.Utils;

namespace ATBS.Repositories;

public class FlightCsv
{
    private const int ExpectedColumns = 10;

    public static Flight FromRow(string[] fields)
    {
        if (fields.Length < ExpectedColumns)
        {
            throw new FormatException($"Expected {ExpectedColumns} columns but found {fields.Length}.");
        }

        return new Flight
        {
            FlightId = ParseHelper.ParseInt(fields[0], "FlightId"),
            DepartureCountry = fields[1].Trim(),
            DepartureAirport = fields[2].Trim(),
            DepartureDate = ParseHelper.ParseDate(fields[3], "DepartureDate"),
            DestinationCountry = fields[4].Trim(),
            ArrivalAirport = fields[5].Trim(),
            ArrivalDate = ParseHelper.ParseDate(fields[6], "ArrivalDate"),
            CabinPrices = new Dictionary<CabinClass, float>
            {
                [CabinClass.Economy] = ParseHelper.ParseFloat(fields[7], "EconomyPrice"),
                [CabinClass.Business] = ParseHelper.ParseFloat(fields[8], "BusinessPrice"),
                [CabinClass.First] = ParseHelper.ParseFloat(fields[9], "FirstPrice")
            }
        };
    }

    public static string ToRow(Flight flight) =>
        string.Join(Constants.Csv.Delimiter, [
            flight.FlightId.ToString(),
            flight.DepartureCountry,
            flight.DepartureAirport,
            flight.DepartureDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            flight.DestinationCountry,
            flight.ArrivalAirport,
            flight.ArrivalDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            flight.CabinPrices[CabinClass.Economy].ToString(),
            flight.CabinPrices[CabinClass.Business].ToString(),
            flight.CabinPrices[CabinClass.First].ToString()
        ]);
}