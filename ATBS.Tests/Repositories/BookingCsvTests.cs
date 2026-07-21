using ATBS.Models;
using ATBS.Repositories;

namespace ATBS.Tests.Repositories;

public class BookingCsvTests
{
    [Fact]
    public void FromRow_ValidFields_ReturnsBooking()
    {
        var fields = new[]
        {
            "1",
            "10",
            "person@example.com",
            "Person Name",
            "Business",
            "250.5",
            "2026-07-20",
            "Booked"
        };

        var booking = BookingCsv.FromRow(fields);

        Assert.Equal(1, booking.BookingId);
        Assert.Equal(10, booking.FlightId);
        Assert.Equal("person@example.com", booking.PassengerEmail);
        Assert.Equal("Person Name", booking.PassengerName);
        Assert.Equal(CabinClass.Business, booking.CabinClass);
        Assert.Equal(250.5f, booking.Price);
        Assert.Equal(new DateTime(2026, 7, 20), booking.BookingDate.Date);
        Assert.Equal(Status.Booked, booking.Status);
    }

    [Fact]
    public void FromRow_WithTooFewFields_ThrowsFormatException()
    {
        var fields = new[] { "1", "10" };

        var exception = Assert.Throws<FormatException>(() => BookingCsv.FromRow(fields));

        Assert.Equal("Expected 8 columns but found 2.", exception.Message);
    }

    [Fact]
    public void ToRow_ReturnsExpectedCsvFormat()
    {
        var booking = new Booking
        {
            BookingId = 3,
            FlightId = 11,
            PassengerEmail = "a@b.com",
            PassengerName = "Alice",
            CabinClass = CabinClass.First,
            Price = 999.99f,
            BookingDate = new DateTime(2026, 7, 20, 14, 30, 0),
            Status = Status.Cancelled
        };

        var row = BookingCsv.ToRow(booking);

        Assert.Equal("3,11,a@b.com,Alice,First,999.99,2026-07-20T14:30:00,Cancelled", row);
    }
}
