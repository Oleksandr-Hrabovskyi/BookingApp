using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Commands;
using BookingApp.Domain.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingApp.Api.Controllers;

[ApiController]
[Route("api/booking")]
[Produces("application/json")]
public class BookingController : BaseController
{
    private readonly IMediator _mediator;
    public BookingController(IMediator mediator, ILogger<BookingController> logger) : base(logger)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Returns Booking by id
    /// </summary>
    /// <param name="bookingId">Booking ID</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Booking with Room ID</returns>
    /// <response code="200">Returns Booking</response>
    /// <response code="404">Booking not found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpGet("{bookingId}")]
    [ProducesResponseType(typeof(BookingResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
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
                    RoomId = booking.RoomId,
                    Room = booking.Room,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate
                }
            };
            return Ok(response);
        }, cancellationToken);

    /// <summary>
    /// Get all Bookings
    /// </summary>
    /// <response code="200">Returns all bookings</response>
    [HttpGet("allBookings")]
    public async Task<IActionResult> GetAllBookings()
    {
        var result = await _mediator.Send(new GetAllBookingsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Create Booking
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Booking ID</returns>
    /// <response code="200">Returns booking ID</response>
    /// <response code="404">Booking not found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut]
    [ProducesResponseType(typeof(CreateBookingResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request,
        CancellationToken cancellationToken) =>
        SaveExecute(async () =>
        {
            if (!ModelState.IsValid)
            {
                return ToActionResult(new ErrorResponse
                {
                    Code = ErrorCode.BadRequest,
                    Message = "Invalid request"
                });
            }
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
            return Created($"http://{Request.Host}/api/booking.com/{response.Id}", response);
        }, cancellationToken);

    /// <summary>
    /// Delete Booking
    /// </summary>
    /// <param name="bookingId"></param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <response code="204">204 No Content</response>
    /// <response code="404">Booking not found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpDelete("{bookingId}")]
    [ProducesResponseType(typeof(DeleteBookingResponse), 204)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public Task<IActionResult> DeleteBooking([FromRoute] int bookingId,
        CancellationToken cancellationToken) =>
        SaveExecute(async () =>
        {
            var command = new DeleteBookingCommand
            {
                BookingId = bookingId
            };

            var result = await _mediator.Send(command, cancellationToken);
            return NoContent();
        }, cancellationToken);
}
