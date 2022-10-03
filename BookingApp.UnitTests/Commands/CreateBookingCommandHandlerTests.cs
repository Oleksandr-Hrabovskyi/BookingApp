using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;
using BookingApp.Domain.Commands;
using BookingApp.Domain.Exceptions;
using BookingApp.UnitTests.Base;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace BookingApp.UnitTests.Commands;

public class CreateBookingCommandHandlerTests : BaseHandlerTest<CreateBookingCommand, CreateBookingCommandResult>
{
    public CreateBookingCommandHandlerTests() : base()
    {
    }

    protected override IRequestHandler<CreateBookingCommand, CreateBookingCommandResult> CreateHandler()
    {
        return new CreateBookingCommandHandler(DbContext,
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

        await DbContext.Room.AddAsync(room);
        await DbContext.SaveChangesAsync();

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
        var result = await Handler.Handle(command, CancellationToken.None);

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
        var command = new CreateBookingCommand
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = Guid.NewGuid().ToString(),
            RoomId = -1,
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21)
        };

        try
        {
            //Act
            await Handler.Handle(command, CancellationToken.None);
            throw new Exception("Exception was expected");
        }
        catch (BookingException be)
        {
            // Assert
            be.ErrorCode.ShouldBe(ErrorCode.RoomNotFound);
            be.Message.ShouldBe($"Room {command.RoomId} not found");
        }
    }
}