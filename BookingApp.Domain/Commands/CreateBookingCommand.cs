using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;
using BookingApp.Domain.Base;
using BookingApp.Domain.Database;
using BookingApp.Domain.Exceptions;
using BookingApp.Domain.Helpers;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookingApp.Domain.Commands;

public class CreateBookingCommand : IRequest<CreateBookingCommandResult>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PhoneNumber { get; init; }
    public int RoomId { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
    public string Comment { get; set; }
}

public class CreateBookingCommandResult
{
    public Booking Booking { get; init; }
}

internal class CreateBookingCommandHandler : BaseHandler<CreateBookingCommand, CreateBookingCommandResult>
{
    private readonly BookingDbContext _dbContext;

    public CreateBookingCommandHandler(BookingDbContext dbContext,
        ILogger<CreateBookingCommandHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<CreateBookingCommandResult> HandleInternal(CreateBookingCommand request,
        CancellationToken cancellationToken)
    {
        var roomId = request.RoomId;

        var room = await _dbContext.Room
            .SingleOrDefaultAsync(r => r.Id == roomId, cancellationToken);

        if (room == null)
        {
            throw new BookingException(ErrorCode.RoomNotFound, $"Room {roomId} not found");
        }

        var booking = new Booking
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            RoomId = roomId,
            CheckInDate = request.CheckInDate.ToUniversalTime(),
            CheckOutDate = request.CheckOutDate.ToUniversalTime(),
            Comment = request.Comment
        };

        if ((string.IsNullOrEmpty(booking.FirstName) ||
            string.IsNullOrEmpty(booking.LastName) ||
            string.IsNullOrEmpty(booking.PhoneNumber) ||
            booking.CheckOutDate <= booking.CheckInDate))
        {
            throw new BookingException(ErrorCode.BadRequest, "Invalid data for booking");
        }
        var validator = new ValidationBooking(_dbContext);
        if (await validator.BookingValidate(booking) == false)
        {

        }

        await _dbContext.AddAsync(booking, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateBookingCommandResult
        {
            Booking = booking
        };

    }
}
