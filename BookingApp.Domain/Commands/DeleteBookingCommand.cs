using System.Threading;
using System.Threading.Tasks;

namespace BookingApp.Domain.Commands;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Database;
using BookingApp.Domain.Exceptions;

using MediatR;

public class DeleteBookingCommand : IRequest
{
    public int BookingId { get; set; }
}



public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
{
    private readonly BookingDbContext _dbContext;

    public DeleteBookingCommandHandler(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingId = request.BookingId;

        var booking = await _dbContext.Booking.FindAsync(bookingId, cancellationToken);

        if (booking == null || booking.Id != bookingId)
        {
            throw new BookingException(ErrorCode.BookingNotFound, $"Booking {bookingId} not found");
        }
        _dbContext.Booking.Remove(booking);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}


