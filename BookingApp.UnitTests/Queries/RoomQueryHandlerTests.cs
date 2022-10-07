using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;
using BookingApp.Domain.Exceptions;
using BookingApp.UnitTests.Base;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using RoomApp.Domain.Queries;

using Shouldly;

namespace BookingApp.UnitTests.Queries;

public class RoomQueryHandlerTests : BaseHandlerTest<RoomQuery, RoomQueryResult>
{
    public RoomQueryHandlerTests() : base()
    {
    }

    protected override IRequestHandler<RoomQuery, RoomQueryResult> CreateHandler()
    {
        return new RoomQueryHandler(DbContext,
            new Mock<ILogger<RoomQueryHandler>>().Object);
    }

    [Fact]
    public async Task HandleShouldReturnRoom()
    {
        // Arrange
        var room = new Room
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };
        await DbContext.Room.AddAsync(room);
        await DbContext.SaveChangesAsync();

        var query = new RoomQuery
        {
            RoomId = room.Id
        };

        // Act
        var result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Room.ShouldNotBeNull();
        result.Room.Id.ShouldBe(room.Id);
        result.Room.Name.ShouldBe(room.Name);
        result.Room.Type.ShouldBe(room.Type);
        result.Room.Price.ShouldBe(room.Price);
    }

    [Fact]
    public async Task HandleShouldThrowExceptionIfNoRoom()
    {
        // Arrange
        var roomId = -1;
        var query = new RoomQuery
        {
            RoomId = roomId
        };

        try
        {
            // Act
            await Handler.Handle(query, CancellationToken.None);
        }
        catch (BookingException be) when (be.ErrorCode == ErrorCode.RoomNotFound
            && be.Message == $"Room {roomId} not found")
        {
            // Assert
            // ignore
        }
    }
}