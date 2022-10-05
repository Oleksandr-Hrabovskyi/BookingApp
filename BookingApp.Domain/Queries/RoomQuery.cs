using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;
using BookingApp.Domain.Base;
using BookingApp.Domain.Database;
using BookingApp.Domain.Exceptions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RoomApp.Domain.Queries;

public class RoomQuery : IRequest<RoomQueryResult>
{
    public int RoomId { get; init; }
}

public class RoomQueryResult
{
    public Room Room { get; set; }
}

internal class RoomQueryHandler : BaseHandler<RoomQuery, RoomQueryResult>
{
    private readonly BookingDbContext _dbContext;

    public RoomQueryHandler(BookingDbContext dbContext, ILogger<RoomQueryHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<RoomQueryResult> HandleInternal(RoomQuery request,
        CancellationToken cancellationToken)
    {
        var RoomId = request.RoomId;

        var Room = await _dbContext.Room
            .SingleOrDefaultAsync(b => b.Id == RoomId, cancellationToken);

        return Room == null
            ? throw new BookingException(ErrorCode.RoomNotFound, $"Room {RoomId} not found")
            : new RoomQueryResult
            {
                Room = Room
            };
    }
}