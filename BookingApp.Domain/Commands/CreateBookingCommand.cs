using System.Threading;
using System.Threading.Tasks;

using BookingApp.Domain.Database;

using MediatR;

namespace BookingApp.Domain.Commands;

public class CreateBookingCommand : IRequest<CreateBookingCommandResult>
{

}

public class CreateBookingCommandResult
{

}

public class CreateBookingCommandResultHandler : IRequestHandler<CreateBookingCommand, CreateBookingCommandResult>
{
    private readonly BookingDbContext _dBContext;
    public Task<CreateBookingCommandResult> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
