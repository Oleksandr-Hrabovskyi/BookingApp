using System.Threading;
using System.Threading.Tasks;

namespace BookingApp.Domain.Commands;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Base;
using BookingApp.Domain.Database;
using BookingApp.Domain.Exceptions;

using MediatR;

using Microsoft.Extensions.Logging;

public class DeleteBookingCommand : IRequest<DeleteBookingCommandResult>
{
    public int BookingId { get; init; }
}

public class DeleteBookingCommandResult
{
    public bool DeleteResult { get; init; }
}

internal class DeleteBookingCommandHandler : BaseHandler<DeleteBookingCommand, DeleteBookingCommandResult>
{
    private readonly BookingDbContext _dbContext;

    public DeleteBookingCommandHandler(BookingDbContext dbContext,
        ILogger<DeleteBookingCommandHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<DeleteBookingCommandResult> HandleInternal(DeleteBookingCommand request,
        CancellationToken cancellationToken)
    {
        var bookingId = request.BookingId;

        var booking = await _dbContext.Booking.FindAsync(new object[] { bookingId }, cancellationToken);
        if (booking == null || booking.Id != bookingId)
        {
            throw new BookingException(ErrorCode.BookingNotFound, $"Booking {bookingId} not found");
        }

        _dbContext.Booking.Remove(booking);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteBookingCommandResult
        {
            DeleteResult = true
        };
    }
}


