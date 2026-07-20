using ATBS.Models;
using ATBS.Utils;

namespace ATBS.Repositories;

public interface IBookingRepository
{
    Task<List<CsvRow>> ReadRawRowsAsync(string path);

    Task<List<Booking>> GetAllAsync();

    Task<Booking?> GetByIdAsync(int bookingId);

    Task<List<Booking>> GetPassengerBookings(string email, string name);

    Task AddAsync(Booking booking);

    Task AddRangeAsync(IEnumerable<Booking> bookings);

    Task UpdateAsync(Booking updated);
}
