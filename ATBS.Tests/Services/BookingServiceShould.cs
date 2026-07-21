using ATBS.Models;
using ATBS.Repositories;
using ATBS.Services;
using Moq;

namespace ATBS.Tests.Services;

public class BookingServiceShould
{
    [Fact]
    public async Task BookFlightAsync_AddsBookingWithNextIdAndFlightPrice()
    {
        var bookings = new Mock<IBookingRepository>();
        var flights = new Mock<IFlightRepository>();
        var service = new BookingService(bookings.Object, flights.Object);
        var flight = CreateFlight();
        var existingBookings = new List<Booking>
        {
            new() { BookingId = 2 },
            new() { BookingId = 7 }
        };

        bookings.Setup(repository => repository.GetAllAsync()).ReturnsAsync(existingBookings);
        flights.Setup(repository => repository.GetByIdAsync(1)).ReturnsAsync(flight);

        var before = DateTime.UtcNow;
        var (booking, returnedFlight) = await service.BookFlightAsync(1, "alice@example.com", "Alice", CabinClass.Business);
        var after = DateTime.UtcNow;

        Assert.Same(flight, returnedFlight);
        Assert.Equal(8, booking.BookingId);
        Assert.Equal(1, booking.FlightId);
        Assert.Equal("alice@example.com", booking.PassengerEmail);
        Assert.Equal("Alice", booking.PassengerName);
        Assert.Equal(CabinClass.Business, booking.CabinClass);
        Assert.Equal(Status.Booked, booking.Status);
        Assert.Equal(250f, booking.Price);
        Assert.InRange(booking.BookingDate, before.AddSeconds(-1), after.AddSeconds(1));

        bookings.Verify(repository => repository.AddAsync(It.Is<Booking>(item =>
            item.BookingId == 8 &&
            item.FlightId == 1 &&
            item.CabinClass == CabinClass.Business &&
            item.Price == 250f &&
            item.Status == Status.Booked)), Times.Once);
    }

    [Fact]
    public async Task BookFlightAsync_ThrowsWhenFlightMissing()
    {
        var bookings = new Mock<IBookingRepository>();
        var flights = new Mock<IFlightRepository>();
        var service = new BookingService(bookings.Object, flights.Object);

        flights.Setup(repository => repository.GetByIdAsync(1)).ReturnsAsync((Flight?)null);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.BookFlightAsync(1, "alice@example.com", "Alice", CabinClass.Economy));

        Assert.Equal("Flight 1 not found", exception.Message);
        bookings.Verify(repository => repository.AddAsync(It.IsAny<Booking>()), Times.Never);
    }

    [Fact]
    public async Task ModifyBookingAsync_UpdatesBookingAndPrice()
    {
        var bookings = new Mock<IBookingRepository>();
        var flights = new Mock<IFlightRepository>();
        var service = new BookingService(bookings.Object, flights.Object);
        var booking = new Booking
        {
            BookingId = 10,
            FlightId = 99,
            Status = Status.Booked,
            CabinClass = CabinClass.Economy
        };
        var flight = CreateFlight();

        bookings.Setup(repository => repository.GetByIdAsync(10)).ReturnsAsync(booking);
        flights.Setup(repository => repository.GetByIdAsync(99)).ReturnsAsync(flight);

        var (updatedBooking, returnedFlight) = await service.ModifyBookingAsync(10, "alice@example.com", "Alice", CabinClass.First);

        Assert.Same(booking, updatedBooking);
        Assert.Same(flight, returnedFlight);
        Assert.Equal(CabinClass.First, booking.CabinClass);
        Assert.Equal(600f, booking.Price);

        bookings.Verify(repository => repository.UpdateAsync(It.Is<Booking>(item =>
            item.BookingId == 10 &&
            item.CabinClass == CabinClass.First &&
            item.Price == 600f)), Times.Once);
    }

    [Fact]
    public async Task ModifyBookingAsync_ThrowsWhenBookingMissing()
    {
        var bookings = new Mock<IBookingRepository>();
        var flights = new Mock<IFlightRepository>();
        var service = new BookingService(bookings.Object, flights.Object);

        bookings.Setup(repository => repository.GetByIdAsync(10)).ReturnsAsync((Booking?)null);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ModifyBookingAsync(10, "alice@example.com", "Alice", CabinClass.First));

        Assert.Equal("Booking 10 not found", exception.Message);
        flights.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ModifyBookingAsync_ThrowsWhenBookingIsCancelled()
    {
        var bookings = new Mock<IBookingRepository>();
        var flights = new Mock<IFlightRepository>();
        var service = new BookingService(bookings.Object, flights.Object);

        bookings.Setup(repository => repository.GetByIdAsync(10)).ReturnsAsync(new Booking
        {
            BookingId = 10,
            FlightId = 99,
            Status = Status.Cancelled
        });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ModifyBookingAsync(10, "alice@example.com", "Alice", CabinClass.First));

        Assert.Equal("Cannot modify a cancelled booking", exception.Message);
        flights.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task CancelBookingAsync_CancelsBooking()
    {
        var bookings = new Mock<IBookingRepository>();
        var flights = new Mock<IFlightRepository>();
        var service = new BookingService(bookings.Object, flights.Object);
        var booking = new Booking
        {
            BookingId = 15,
            FlightId = 99,
            Status = Status.Booked
        };

        bookings.Setup(repository => repository.GetByIdAsync(15)).ReturnsAsync(booking);

        var result = await service.CancelBookingAsync(15, "alice@example.com", "Alice");

        Assert.Same(booking, result);
        Assert.Equal(Status.Cancelled, booking.Status);
        bookings.Verify(repository => repository.UpdateAsync(It.Is<Booking>(item =>
            item.BookingId == 15 && item.Status == Status.Cancelled)), Times.Once);
    }

    [Fact]
    public async Task CancelBookingAsync_ThrowsWhenAlreadyCancelled()
    {
        var bookings = new Mock<IBookingRepository>();
        var flights = new Mock<IFlightRepository>();
        var service = new BookingService(bookings.Object, flights.Object);

        bookings.Setup(repository => repository.GetByIdAsync(15)).ReturnsAsync(new Booking
        {
            BookingId = 15,
            FlightId = 99,
            Status = Status.Cancelled
        });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CancelBookingAsync(15, "alice@example.com", "Alice"));

        Assert.Equal("Booking is already cancelled", exception.Message);
        bookings.Verify(repository => repository.UpdateAsync(It.IsAny<Booking>()), Times.Never);
    }

    private static Flight CreateFlight() => new()
    {
        FlightId = 1,
        DepartureCountry = "Egypt",
        DepartureAirport = "CAI",
        DepartureDate = DateTime.Today.AddDays(1),
        DestinationCountry = "France",
        ArrivalAirport = "CDG",
        ArrivalDate = DateTime.Today.AddDays(2),
        CabinPrices = new Dictionary<CabinClass, float>
        {
            [CabinClass.Economy] = 100f,
            [CabinClass.Business] = 250f,
            [CabinClass.First] = 600f
        }
    };
}
