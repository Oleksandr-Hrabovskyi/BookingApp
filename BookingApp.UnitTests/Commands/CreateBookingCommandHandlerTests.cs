using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Domain.Commands;
using BookingApp.Domain.Database;
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
        var bookingFirstName = Guid.NewGuid().ToString();
        var bookingLastName = Guid.NewGuid().ToString();
        var bookingPhoneNumber = Guid.NewGuid().ToString();
        var roomName = Guid.NewGuid().ToString();
        // var roomType = Guid.NewGuid().ToString();
        // var roomPrice = new Random();
        var roomId = new Random().Next(0, 100);
        // var BookingRoom = new Room
        // {
        //     Name = RoomName,
        //     Type = RoomType,
        //     Price = RoomPrice.Next(1000, 2500)
        // };
        var bookingCheckInDate = new DateTime(2022, 9, 20);
        var bookingCheckOutDate = new DateTime(2022, 9, 21);

        var command = new CreateBookingCommand
        {
            FirstName = bookingFirstName,
            LastName = bookingLastName,
            PhoneNumber = bookingPhoneNumber,
            RoomId = roomId,
            CheckInDate = bookingCheckInDate,
            CheckOutDate = bookingCheckOutDate
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
        //result.Booking.Room.Name.ShouldNotBeNull();
        //result.Booking.Room.Type.ShouldNotBeNull();
        //result.Booking.Room.Price.ShouldBeGreaterThan(0);
    }

    // public void Dispose()
    // {
    //     _dbContext.Database.EnsureDeleted();
    //     _dbContext.Dispose();
    // }
}