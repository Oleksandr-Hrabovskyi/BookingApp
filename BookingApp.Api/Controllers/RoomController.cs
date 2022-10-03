using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingApp.Api.Controllers;

[ApiController]
[Route("api/room")]
[Produces("application/json")]
public class RoomController : BaseController
{
    private readonly IMediator _mediator;
    public RoomController(IMediator mediator, ILogger<RoomController> logger) : base(logger)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create Room
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Room ID</returns>
    /// <response code="200">Returns Room ID</response>
    /// <response code="404">Room not found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut]
    [ProducesResponseType(typeof(CreateRoomResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request,
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
            var command = new CreateRoomCommand
            {
                Name = request.Name,
                Type = request.Type,
                Price = request.Price
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new CreateRoomResponse
            {
                Id = result.Room.Id
            };
            return Created($"http://{Request.Host}/api/room.com/{response.Id}", response);
        }, cancellationToken);
}