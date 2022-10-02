using System;
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

    public CreateBookingCommandHandler(BookingDbContext dBContext,
        ILogger<CreateBookingCommandHandler> logger) : base(logger)
    {
        _dbContext = dBContext;
    }

    protected override async Task<CreateBookingCommandResult> HandleInternal(CreateBookingCommand request,
        CancellationToken cancellationToken)
    {
        var roomId = request.RoomId;

        var room = await _dbContext.Room
            .SingleOrDefaultAsync(r => r.Id == roomId, cancellationToken);

        if (room == null)
        {
            throw new BookingException(ErrorCode.BookingNotFound, $"Booking {roomId} not found");
        }

        var booking = new Booking
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            RoomId = roomId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            Comment = request.Comment
        };
        await _dbContext.AddAsync(booking, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateBookingCommandResult
        {
            Booking = booking
        };
    }
}
