using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;
using BookingApp.Domain.Exceptions;
using BookingApp.Domain.Queries;
using BookingApp.UnitTests.Base;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace BookingApp.UnitTests.Queries;

public class GetAllRoomsQueryHandlerTests : BaseHandlerTest<GetAllRoomsQuery, GetAllRoomsQueryResult>
{
    public GetAllRoomsQueryHandlerTests() : base()
    {
    }

    protected override IRequestHandler<GetAllRoomsQuery, GetAllRoomsQueryResult> CreateHandler()
    {
        return new GetAllRoomsQueryHandler(DbContext,
            new Mock<ILogger<GetAllRoomsQueryHandler>>().Object);
    }

    [Fact]
    public async Task HandleShouldReturnAllRooms()
    {
        // Arrange
        var room1 = new Room
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };
        await DbContext.Room.AddAsync(room1);
        await DbContext.SaveChangesAsync();

        var room2 = new Room
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };
        await DbContext.Room.AddAsync(room2);
        await DbContext.SaveChangesAsync();

        var rooms = new List<Room>()
        {
            room1,
            room2
        };

        var query = new GetAllRoomsQuery
        {
            RoomId = room1.Id
        };

        // Act
        var result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Rooms.ShouldNotBeNull();

    }

    [Fact]
    public async Task HandleShouldThrowExceptionIfNoAllRooms()
    {
        // Arrange
        var roomId = -1;
        var query = new GetAllRoomsQuery
        {
            RoomId = roomId
        };

        try
        {
            // Act
            await Handler.Handle(query, CancellationToken.None);
        }
        catch (BookingException be) when (be.ErrorCode == ErrorCode.RoomNotFound
            && be.Message == $"Rooms {roomId} not found")
        {
            // Assert
            // ignore
        }
    }
}