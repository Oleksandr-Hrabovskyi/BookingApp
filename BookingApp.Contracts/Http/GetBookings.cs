using BookingApp.Contracts.Database;

namespace BookingApp.Contracts.Http;

public class BookingResponse
{
    public Booking Booking { get; init; }
}