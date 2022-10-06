using System.Collections.Generic;

using BookingApp.Contracts.Database;

namespace BookingApp.Contracts.Http;

public class GetAllBookingsQueryResult
{
    public List<Booking> Bookings { get; init; }
}