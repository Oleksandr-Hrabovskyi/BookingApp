using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Domain.Base;
using BookingApp.Domain.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookingApp.Domain.Queries;

public class GetAllRoomsQuery : IRequest<GetAllRoomsQueryResult>
{
    public int RoomId { get; init; }
}

public class GetAllRoomsQueryResult
{
    public List<Room> Rooms { get; init; }
}

internal class GetAllRoomsQueryHandler : BaseHandler<GetAllRoomsQuery, GetAllRoomsQueryResult>
{
    private readonly BookingDbContext _dbContext;

    public GetAllRoomsQueryHandler(BookingDbContext dbContext,
        ILogger<GetAllRoomsQueryHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<GetAllRoomsQueryResult> HandleInternal(GetAllRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var rooms = await _dbContext.Room.ToListAsync(cancellationToken);
        // var allBookings = rooms.ToList();
        return new GetAllRoomsQueryResult
        {
            Rooms = rooms
        };
    }
}