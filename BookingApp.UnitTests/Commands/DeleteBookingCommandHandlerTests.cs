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

public class DeleteBookingCommandHandlerTests : BaseHandlerTest<DeleteBookingCommand, DeleteBookingCommandResult>
{
    public DeleteBookingCommandHandlerTests() : base()
    {
    }

    protected override IRequestHandler<DeleteBookingCommand, DeleteBookingCommandResult> CreateHandler()
    {
        return new DeleteBookingCommandHandler(DbContext,
            new Mock<ILogger<DeleteBookingCommandHandler>>().Object);
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

        var booking = new Booking
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = Guid.NewGuid().ToString(),
            RoomId = room.Id,
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21)
        };

        await DbContext.Booking.AddAsync(booking);
        await DbContext.SaveChangesAsync();

        //Act
        var command = new DeleteBookingCommand
        {
            BookingId = booking.Id
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

        var booking = new Booking
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = Guid.NewGuid().ToString(),
            RoomId = room.Id,
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21)
        };

        await DbContext.Booking.AddAsync(booking);
        await DbContext.SaveChangesAsync();

        //Act
        var command = new DeleteBookingCommand
        {
            BookingId = -1
        };
        var result = await Handler.Handle(command, CancellationToken.None);

        //Assert
        result.ShouldNotBeNull();
        result.DeleteSuccess.ShouldBeFalse();
    }
}