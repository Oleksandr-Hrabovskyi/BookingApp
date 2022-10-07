using System;

namespace BookingApp.Contracts.Http;

public class CreateBookingRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PhoneNumber { get; init; }
    public int RoomId { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
    public string Comment { get; init; }
}

public class CreateBookingResponse
{
    public int Id { get; init; }
}