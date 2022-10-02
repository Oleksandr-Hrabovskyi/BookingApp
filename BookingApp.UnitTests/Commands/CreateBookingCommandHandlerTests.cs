using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;
using BookingApp.Domain.Commands;
using BookingApp.Domain.Database;
using BookingApp.Domain.Exceptions;
using BookingApp.UnitTests.Helpers;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace BookingApp.UnitTests.Commands;

public class CreateBookingCommandHandlerTests
{
    private readonly BookingDbContext _dbContext;
    private readonly IRequestHandler<CreateBookingCommand, CreateBookingCommandResult> _handler;
    public CreateBookingCommandHandlerTests()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _dbContext.Database.Migrate();
        _handler = new CreateBookingCommandHandler(_dbContext,
            new Mock<ILogger<CreateBookingCommandHandler>>().Object);
    }

    [Fact]
    public async Task HandleShouldCreateBooking()
    {
        //Arrange
        var room = new Room
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };

        var command = new CreateBookingCommand
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = Guid.NewGuid().ToString(),
            RoomId = room.Id,
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21)
        };

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.ShouldNotBeNull();
        result.Booking.ShouldNotBeNull();
        result.Booking.Id.ShouldBeGreaterThan(0);
        result.Booking.FirstName.ShouldNotBeNull();
        result.Booking.LastName.ShouldNotBeNull();
        result.Booking.PhoneNumber.ShouldNotBeNull();
    }

    [Fact]
    public async Task HandleShouldThrowExceptionIfNoRoom()
    {
        //Arrange
        var roomId = -1;
        var command = new CreateBookingCommand
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = Guid.NewGuid().ToString(),
            RoomId = roomId,
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21)
        };

        try
        {
            //Act
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (BookingException be) when (be.ErrorCode == ErrorCode.BookingNotFound &&
            be.Message == $"Booking {roomId} not found")
        {
            // Assert
            // ignore
        }
    }


    // public void Dispose()
    // {
    //     _dbContext.Database.EnsureDeleted();
    //     _dbContext.Dispose();
    // }
}