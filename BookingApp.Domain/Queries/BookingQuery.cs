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

namespace BookingApp.Domain.Queries;

public class BookingQuery : IRequest<BookingQueryResult>
{
    public int BookingId { get; init; }
}

public class BookingQueryResult
{
    public Booking Booking { get; set; }
}

internal class BookingQueryHandler : BaseHandler<BookingQuery, BookingQueryResult>
{
    private readonly BookingDbContext _dbContext;

    public BookingQueryHandler(BookingDbContext dbContext, ILogger<BookingQueryHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<BookingQueryResult> HandleInternal(BookingQuery request,
        CancellationToken cancellationToken)
    {
        var bookingId = request.BookingId;

        var booking = await _dbContext.Booking
            .SingleOrDefaultAsync(b => b.Id == bookingId, cancellationToken);

        return booking == null
            ? throw new BookingException(ErrorCode.BookingNotFound, $"Booking {bookingId} not found")
            : new BookingQueryResult
            {
                Booking = booking
            };
    }
}