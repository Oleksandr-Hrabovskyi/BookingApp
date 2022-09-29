using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Commands;
using BookingApp.Domain.Queries;

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

    [HttpGet("{bookingId}")]
    public Task<IActionResult> GetBooking([FromRoute] int bookingId,
        CancellationToken cancellationToken) =>
        SaveExecute(async () =>
        {
            var query = new BookingQuery
            {
                BookingId = bookingId
            };

            var result = await _mediator.Send(query, cancellationToken);
            var booking = result.Booking;
            var response = new BookingResponse
            {
                Booking = new Contracts.Database.Booking
                {
                    Id = booking.Id,
                    FirstName = booking.FirstName,
                    LastName = booking.LastName,
                    PhoneNumber = booking.PhoneNumber,
                    RoomId = booking.Id,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate
                }
            };
            return Ok(response);
        }, cancellationToken);

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