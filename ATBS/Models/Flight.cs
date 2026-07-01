using System.ComponentModel.DataAnnotations;
using ATBS.Attributes;

namespace ATBS.Models;

public enum CabinClass
{
    Economy,
    Business,
    First
}

public class Flight
{
    public required Guid FlightId { get; set; }
    
    [Required]
    [StringLength(60)]
    public string DepartureCountry { get; set; } = string.Empty;
    
    [Required]
    [StringLength(10)]
    public string DepartureAirport { get; set; } = string.Empty;
    
    [Required]
    [NotInThePast]
    public DateTime DepartureDate { get; set; }

    [Required]
    [StringLength(60)]
    public string DestinationCountry { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string ArrivalAirport { get; set; } = string.Empty;
    
    [Required]
    [NotInThePast]
    public DateTime ArrivalDate { get; set; }

    [Required]
    [ValidCabinPrices]
    public required Dictionary<CabinClass, float> CabinPrices { get; set; }

    public override string ToString() =>
        $"[{FlightId}]\n" +
        $"{DepartureCountry} ({DepartureAirport}) -> {DestinationCountry} ({ArrivalAirport})" +
        $"on {DepartureDate:yyyy-MM-dd} | Eco {CabinPrices[CabinClass.Economy]} / Bus {CabinPrices[CabinClass.Business]} / First {CabinPrices[CabinClass.First]}";

}