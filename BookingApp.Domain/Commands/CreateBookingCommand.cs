using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Domain.Base;
using BookingApp.Domain.Database;

using MediatR;

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
    private readonly BookingDbContext _dBContext;

    public CreateBookingCommandHandler(BookingDbContext dBContext,
        ILogger<CreateBookingCommandHandler> logger) : base(logger)
    {
        _dBContext = dBContext;
    }

    protected override async Task<CreateBookingCommandResult> HandleInternal(CreateBookingCommand request,
        CancellationToken cancellationToken)
    {
        // Logger.LogDebug("Start to execute CreateBookingCommand with parameters {FirstName}, {LastName}, {RoomId}, {PhoneNumber}, {CheckInDate}, {CheckOutDate}",
        //     request.FirstName, request.LastName, request.RoomId, request.PhoneNumber, request.CheckInDate, request.CheckOutDate);

        var booking = new Booking
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            RoomId = request.RoomId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            Comment = request.Comment
        };
        await _dBContext.AddAsync(booking, cancellationToken);
        await _dBContext.SaveChangesAsync(cancellationToken);

        //Logger.LogInformation("Created new booking with id {Id}", booking.Id);

        return new CreateBookingCommandResult
        {
            Booking = booking
        };
    }
}
