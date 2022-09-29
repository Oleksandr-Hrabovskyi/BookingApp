using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;
using BookingApp.Domain.Database;
using BookingApp.Domain.Exceptions;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace BookingApp.Domain.Queries;

public class BookingQuery : IRequest<BookingQueryResult>
{
    public int BookingId { get; init; }
}

public class BookingQueryResult
{
    public Booking Booking { get; set; }
}

public class BookingQueryHandler : IRequestHandler<BookingQuery, BookingQueryResult>
{
    private readonly BookingDbContext _dbContext;

    public BookingQueryHandler(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BookingQueryResult> Handle(BookingQuery request, CancellationToken cancellationToken)
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