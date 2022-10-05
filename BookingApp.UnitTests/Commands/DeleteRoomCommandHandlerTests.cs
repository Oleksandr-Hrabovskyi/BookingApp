using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Domain.Commands;
using BookingApp.UnitTests.Base;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace BookingApp.UnitTests.Commands;

public class DeleteRoomCommandHandlerTests : BaseHandlerTest<DeleteRoomCommand, DeleteRoomCommandResult>
{
    public DeleteRoomCommandHandlerTests() : base()
    {
    }

    protected override IRequestHandler<DeleteRoomCommand, DeleteRoomCommandResult> CreateHandler()
    {
        return new DeleteRoomCommandHandler(DbContext,
            new Mock<ILogger<DeleteRoomCommandHandler>>().Object);
    }

    [Fact]
    public async Task HandleShouldDeleteBooking()
    {
        //Arrange
        var room = new Room
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };

        await DbContext.Room.AddAsync(room);
        await DbContext.SaveChangesAsync();

        //Act
        var command = new DeleteRoomCommand
        {
            RoomId = room.Id
        };
        var result = await Handler.Handle(command, CancellationToken.None);

        //Assert
        result.ShouldNotBeNull();
        result.DeleteSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task HandleShouldNotDeleteBooking()
    {
        //Arrange
        var room = new Room
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };

        await DbContext.Room.AddAsync(room);
        await DbContext.SaveChangesAsync();

        //Act
        var command = new DeleteRoomCommand
        {
            RoomId = -1
        };
        var result = await Handler.Handle(command, CancellationToken.None);

        //Assert
        result.ShouldNotBeNull();
        result.DeleteSuccess.ShouldBeFalse();
    }

    public new void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}