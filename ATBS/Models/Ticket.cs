namespace ATBS.Models;

public enum Status
{
    Booked,
    Cancelled
}

public class Ticket
{
    public Guid TicketId { get; set; }
    public Guid FlightId { get; set; }
    public string PassengerEmail { get; set; } = string.Empty;
    public CabinClass CabinClass { get; set; }
    public decimal Price { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.Now;
    public Status Status { get; set; } = Status.Booked;
}