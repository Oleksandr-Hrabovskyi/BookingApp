using System;
using System.ComponentModel.DataAnnotations;

using BookingApp.Contracts.Database;

namespace BookingApp.Contracts.Http;

public class CreateBookingRequest
{
    [Required]
    [MaxLength(255)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(255)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }

    [Required]
    public Room Room { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [MaxLength(500)]
    public string Comment { get; set; }
}

public class CreateBookingResponse
{
    public int Id { get; set; }
}