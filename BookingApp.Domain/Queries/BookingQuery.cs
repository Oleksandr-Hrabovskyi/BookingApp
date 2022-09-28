using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;

using MediatR;

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
    public Task<BookingQueryResult> Handle(BookingQuery request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}