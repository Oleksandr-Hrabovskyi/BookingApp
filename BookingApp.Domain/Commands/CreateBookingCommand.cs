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
    public Room Room { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
}

public class CreateBookingCommandResult
{
    public int BookingId { get; init; }
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
            Room = request.Room,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate
        };
        await _dBContext.AddAsync(booking, cancellationToken);
        await _dBContext.SaveChangesAsync(cancellationToken);

        return new CreateBookingCommandResult
        {
            BookingId = booking.Id
        };
    }
}
