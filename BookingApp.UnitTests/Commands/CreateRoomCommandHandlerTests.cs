using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Domain.Commands;
using BookingApp.Domain.Database;
using BookingApp.UnitTests.Helpers;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace BookingApp.UnitTests.Commands;

public class CreateRoomCommandHandlerTests
{
    private readonly BookingDbContext _dbContext;
    private readonly IRequestHandler<CreateRoomCommand, CreateRoomCommandResult> _handler;
    public CreateRoomCommandHandlerTests()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _dbContext.Database.Migrate();
        _handler = new CreateRoomCommandHandler(_dbContext,
            new Mock<ILogger<CreateRoomCommandHandler>>().Object);
    }

    [Fact]
    public async Task HandleShouldAddRoomToBooking()
    {
        // Arrange
        var room = new Room
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };
        var booking = new Booking
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = "+380991234567",
            RoomId = room.Id,
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21),
            Comment = Guid.NewGuid().ToString()
        };

        await _dbContext.Booking.AddAsync(booking);
        await _dbContext.SaveChangesAsync();

        var command = new CreateRoomCommand
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };


        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Room.ShouldNotBeNull();
        result.Room.Id.ShouldBeGreaterThan(0);
        result.Room.Name.ShouldBe(command.Name);
        result.Room.Type.ShouldBe(command.Type);
        result.Room.Price.ShouldBe(command.Price);

    }

}