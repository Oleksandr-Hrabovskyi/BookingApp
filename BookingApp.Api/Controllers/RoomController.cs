using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingApp.Api.Controllers;

[Route("api/room")]
public class RoomController : BaseController
{
    private readonly IMediator _mediator;
    public RoomController(IMediator mediator, ILogger<RoomController> logger) : base(logger)
    {
        _mediator = mediator;
    }

    [HttpPut]
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