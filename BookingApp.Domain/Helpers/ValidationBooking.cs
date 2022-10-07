using System.Linq;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;
using BookingApp.Domain.Database;
using BookingApp.Domain.Exceptions;
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
        var result = true;
        var bookings = await _dbContext.Booking.ToListAsync();
        var overlaps = bookings
            .Where(r => r.RoomId.Equals(booking.RoomId))
            .Any(b => booking.CheckInDate >= b.CheckInDate && booking.CheckInDate < b.CheckOutDate ||
                b.CheckInDate >= booking.CheckInDate && b.CheckInDate < booking.CheckOutDate);

        if (overlaps)
        {
            throw new BookingException(ErrorCode.BadRequest, "Booking overlaps with another");
        }
        return result;
    }
}
