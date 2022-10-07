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

public class GetAllBookingsQuery : IRequest<GetAllBookingsQueryResult>
{
    public int BookingId { get; init; }
}

public class GetAllBookingsQueryResult
{
    public List<Booking> Bookings { get; init; }
}

internal class GetAllBookingsQueryHandler : BaseHandler<GetAllBookingsQuery, GetAllBookingsQueryResult>
{
    private readonly BookingDbContext _dbContext;

    public GetAllBookingsQueryHandler(BookingDbContext dbContext,
        ILogger<GetAllBookingsQueryHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<GetAllBookingsQueryResult> HandleInternal(GetAllBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var bookings = await _dbContext.Booking.ToListAsync(cancellationToken);
        return new GetAllBookingsQueryResult
        {
            Bookings = bookings
        };
    }
}