using System.Threading;
using System.Threading.Tasks;

namespace BookingApp.Domain.Commands;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Base;
using BookingApp.Domain.Database;
using BookingApp.Domain.Exceptions;

using MediatR;

using Microsoft.Extensions.Logging;

public class DeleteRoomCommand : IRequest<DeleteRoomCommandResult>
{
    public int RoomId { get; init; }
}

public class DeleteRoomCommandResult
{
}

internal class DeleteRoomCommandHandler : BaseHandler<DeleteRoomCommand, DeleteRoomCommandResult>
{
    private readonly BookingDbContext _dbContext;

    public DeleteRoomCommandHandler(BookingDbContext dbContext,
        ILogger<DeleteRoomCommandHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<DeleteRoomCommandResult> HandleInternal(DeleteRoomCommand request,
        CancellationToken cancellationToken)
    {
        var roomId = request.RoomId;

        var room = await _dbContext.Room.FindAsync(new object[] { roomId }, cancellationToken);
        if (room == null || room.Id != roomId)
        {
            throw new BookingException(ErrorCode.RoomNotFound, $"Room {roomId} not found");
        }

        _dbContext.Room.Remove(room);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteRoomCommandResult { };
    }
}


