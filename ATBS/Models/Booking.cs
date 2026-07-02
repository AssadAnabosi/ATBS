namespace ATBS.Models;

public enum Status
{
    Booked,
    Cancelled
}

public class Booking
{
    public int BookingId { get; set; }
    public int FlightId { get; set; }
    public string PassengerEmail { get; set; } = string.Empty;
    public string PassengerName { get; set; } = string.Empty;
    public CabinClass CabinClass { get; set; }
    public float Price { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.Now;
    public Status Status { get; set; } = Status.Booked;

    public override string ToString()
    {
        return $"[{BookingId}] - Status: {Status} " +
               $"by {PassengerName} <{PassengerEmail}> " +
               $"Flying {CabinClass} for ${Price}";
    }
}