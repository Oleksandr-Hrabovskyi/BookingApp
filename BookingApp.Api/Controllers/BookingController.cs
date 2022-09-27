using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Api.Controllers;

[Route("api/booking")]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateBookingCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            Room = request.Room
        };

        var result = await _mediator.Send(command, cancellationToken);
        var response = new CreateBookingResponse
        {
            Id = result.Booking.Id
        };
        return Created("http://booking.com", response);
    }
}