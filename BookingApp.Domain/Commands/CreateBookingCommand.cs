using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Domain.Database;

using MediatR;

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

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, CreateBookingCommandResult>
{
    private readonly BookingDbContext _dBContext;

    public CreateBookingCommandHandler(BookingDbContext dBContext)
    {
        _dBContext = dBContext;
    }

    public async Task<CreateBookingCommandResult> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
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

        return new CreateBookingCommandResult
        {
            Booking = booking
        };
    }
}
