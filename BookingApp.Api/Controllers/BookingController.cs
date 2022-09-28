using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Npgsql;

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
        try
        {
            var command = new CreateBookingCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                Room = request.Room,
                Comment = request.Comment
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new CreateBookingResponse
            {
                Id = result.Booking.Id
            };
            return Created("http://booking.com", response);
        }
        catch (InvalidOperationException ioe) when (ioe.InnerException is NpgsqlException)
        {
            var response = new ErrorResponse
            {
                Code = ErrorCode.DbFailureError,
                Message = "DB failure"
            };
            return ToActionResult(response);
        }
        catch (Exception)
        {
            var response = new ErrorResponse
            {
                Code = ErrorCode.InternalServerError,
                Message = "Unhandled error"
            };
            return ToActionResult(response);
        }
    }
    private IActionResult ToActionResult(ErrorResponse errorResponse)
    {
        return StatusCode((int)errorResponse.Code / 100, errorResponse);
    }
}