using System.Xml;
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
            FlightId = ParseHelper.ParseGuid(fields[0], "FlightId"),
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
}