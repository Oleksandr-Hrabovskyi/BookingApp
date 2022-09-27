using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Domain.Commands;
using BookingApp.Domain.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shouldly;

namespace BookingApp.UnitTests.Commands;

public class CreateBookingCommandHandlerTests : IDisposable
{
    private readonly BookingDbContext _dbContext;
    private readonly IRequestHandler<CreateBookingCommand, CreateBookingCommandResult> _handler;
    public CreateBookingCommandHandlerTests()
    {
        var tempfile = Path.GetTempFileName();
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseSqlite("Data Source={tempfile};")
            .Options;

        _dbContext = new BookingDbContext(options);
        _dbContext.Database.Migrate();
        _handler = new CreateBookingCommandHandler(_dbContext);
    }

    [Fact]
    public async Task HandleShouldCreateBooking()
    {
        //Arrange
        var BookingFirstName = Guid.NewGuid().ToString();
        var BookingLastName = Guid.NewGuid().ToString();
        var BookingPhoneNumber = Guid.NewGuid().ToString();
        var RoomName = Guid.NewGuid().ToString();
        var RoomType = Guid.NewGuid().ToString();
        var RoomPrice = new Random();
        var BookingRoom = new Room
        {
            Name = RoomName,
            Type = RoomType,
            Price = RoomPrice.Next()
        };
        var BookingCheckInDate = new DateTime(2022, 9, 20);
        var BookingCheckOutDate = new DateTime(2022, 9, 21);

        var command = new CreateBookingCommand
        {
            FirstName = BookingFirstName,
            LastName = BookingLastName,
            PhoneNumber = BookingPhoneNumber,
            Room = BookingRoom,
            CheckInDate = BookingCheckInDate,
            CheckOutDate = BookingCheckOutDate
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
        result.Booking.Room.Name.ShouldNotBeNull();
        result.Booking.Room.Type.ShouldNotBeNull();
        result.Booking.Room.Price.ShouldBeGreaterThan(0);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}