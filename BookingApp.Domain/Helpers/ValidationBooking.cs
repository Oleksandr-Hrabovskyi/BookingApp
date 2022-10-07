using System.Linq;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Domain.Database;
using BookingApp.Domain.Helpers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace BookingApp.Domain.Helpers;

public class ValidationBooking : IValidationBooking
{
    private readonly BookingDbContext _dbContext;

    public ValidationBooking(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> BookingValidate(Booking booking)
    {
        var bookings = await _dbContext.Booking.ToListAsync();
        var overlaps = bookings
            .Where(r => r.RoomId.Equals(booking.RoomId))
            .Any(b => booking.CheckInDate >= b.CheckInDate && booking.CheckInDate < b.CheckOutDate ||
                b.CheckInDate >= booking.CheckInDate && b.CheckInDate < booking.CheckOutDate);
        return overlaps;
    }
}
