using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Api.Controllers;

[Route("api/booking")]
public class BookingController : BaseController
{
    private readonly IMediator _mediator;

    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request,
        CancellationToken cancellationToken) =>
        SaveExecute(async () =>
        {
            var command = new CreateBookingCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                RoomId = request.RoomId,
                Comment = request.Comment
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new CreateBookingResponse
            {
                Id = result.Booking.Id
            };
            return Created("http://booking.com", response);
        }, cancellationToken);

}