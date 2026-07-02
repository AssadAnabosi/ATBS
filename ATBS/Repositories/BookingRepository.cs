using ATBS.Utils;
using ATBS.Models;

namespace ATBS.Repositories;

public class BookingRepository
{
    public Task<List<CsvRow>> ReadRawRowsAsync(string path) =>
        CsvHelper.ReadRowsAsync(path);

    public async Task<List<Booking>> GetAllAsync()
    {
        var rows = await CsvHelper.ReadRowsAsync(Constants.Files.Bookings);
        return rows.Select(row => BookingCsv.FromRow(row.Fields)).ToList();
    }

    public async Task<Booking?> GetByIdAsync(int bookingId) =>
        (await GetAllAsync()).FirstOrDefault(booking => booking.BookingId == bookingId);

    public async Task<List<Booking>> GetPassengerBookings(string email, string name) =>
        (await GetAllAsync()).Where(booking =>
            booking.PassengerEmail == email && booking.PassengerName == name).ToList();

    public async Task AddAsync(Booking booking)
    {
        await AddRangeAsync([booking]);
    }

    public async Task AddRangeAsync(IEnumerable<Booking> bookings)
    {
        var all = (await GetAllAsync()).Concat(bookings);
        await CsvHelper.WriteAsync(Constants.Files.Bookings, Constants.Csv.BookingHeader, all, BookingCsv.ToRow);
    }

    public async Task UpdateAsync(Booking updated)
    {
        var all = await GetAllAsync();
        var index = all.FindIndex(b => b.BookingId == updated.BookingId);
        if (index < 0)
            throw new InvalidOperationException($"Booking {updated.BookingId} not found");
        all[index] = updated;
        await CsvHelper.WriteAsync(Constants.Files.Bookings, Constants.Csv.BookingHeader, all, BookingCsv.ToRow);
    }
}