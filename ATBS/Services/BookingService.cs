using ATBS.Models;
using ATBS.Repositories;

namespace ATBS.Services;

public class BookingService
{
    private readonly FlightRepository _flights;
    private readonly BookingRepository _bookings;

    public BookingService(BookingRepository bookings, FlightRepository flights)
    {
        _bookings = bookings;
        _flights = flights;
    }
    
    public Task<List<Booking>> GetAllAsync() => _bookings.GetAllAsync();
    
    public Task<Booking> GetByIdAsync(int id) => _bookings.GetByIdAsync(id);
    
    public Task<List<Booking>> GetPassengerBookings(string passengerEmail, string passengerName) => _bookings.GetPassengerBookings(passengerEmail, passengerName);

    public async Task<(Booking, Flight)> BookFlightAsync(int flightId, string passengerEmail, string passengerName,
        CabinClass cabinClass)
    {
        var flight = await _flights.GetByIdAsync(flightId)
                     ?? throw new InvalidOperationException($"Flight {flightId} not found");

        var bookings = await _bookings.GetAllAsync();
        int nextId = bookings.Count == 0 ? 1 : bookings.Max(booking => booking.BookingId) + 1;
        var booking = new Booking
        {
            BookingId = nextId,
            FlightId = flightId,
            PassengerEmail = passengerEmail,
            PassengerName = passengerName,
            CabinClass = cabinClass,
            Status = Status.Booked,
            BookingDate = DateTime.UtcNow,
            Price = flight.CabinPrices[cabinClass]
        };

        await _bookings.AddAsync(booking);
        return (booking, flight);
    }
}