using System;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Contracts.Http;

public class CreateBookingRequest
{
    [Required]
    [MaxLength(255)]
    public string FirstName { get; init; }

    [Required]
    [MaxLength(255)]
    public string LastName { get; init; }

    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; init; }

    [Required]
    public int RoomId { get; init; }

    [Required]
    public DateTime CheckInDate { get; init; }

    [Required]
    public DateTime CheckOutDate { get; init; }

    [MaxLength(500)]
    public string Comment { get; init; }
}

public class CreateBookingResponse
{
    public int Id { get; init; }
}