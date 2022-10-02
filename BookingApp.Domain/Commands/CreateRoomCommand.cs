using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Domain.Base;
using BookingApp.Domain.Database;

using MediatR;

using Microsoft.Extensions.Logging;

namespace BookingApp.Domain.Commands;

public class CreateRoomCommand : IRequest<CreateRoomCommandResult>
{
    public string Name { get; init; }
    public string Type { get; init; }
    public decimal Price { get; init; }
}

public class CreateRoomCommandResult
{
    public Room Room { get; set; }
}

internal class CreateRoomCommandHandler : BaseHandler<CreateRoomCommand, CreateRoomCommandResult>
{
    private readonly BookingDbContext _dbContext;

    public CreateRoomCommandHandler(BookingDbContext dbContext, ILogger<CreateRoomCommandHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<CreateRoomCommandResult> HandleInternal(CreateRoomCommand request,
        CancellationToken cancellationToken)
    {
        var room = new Room
        {
            Name = request.Name,
            Type = request.Type,
            Price = request.Price
        };
        await _dbContext.AddAsync(room, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateRoomCommandResult
        {
            Room = room
        };
    }
}
