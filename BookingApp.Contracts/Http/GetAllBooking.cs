using System.Collections.Generic;

using BookingApp.Contracts.Database;

namespace BookingApp.Contracts.Http;

public class GetAllBookingQueryResult
{
    public List<Booking> Bookings { get; init; }
}