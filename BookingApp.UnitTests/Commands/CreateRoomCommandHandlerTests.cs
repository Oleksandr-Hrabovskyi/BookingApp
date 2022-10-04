using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Domain.Commands;
using BookingApp.UnitTests.Base;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace BookingApp.UnitTests.Commands;

public class CreateRoomCommandHandlerTests : BaseHandlerTest<CreateRoomCommand, CreateRoomCommandResult>
{
    private new readonly IRequestHandler<CreateRoomCommand, CreateRoomCommandResult> Handler;
    public CreateRoomCommandHandlerTests() : base()
    {
    }

    protected override IRequestHandler<CreateRoomCommand, CreateRoomCommandResult> CreateHandler()
    {
        return new CreateRoomCommandHandler(DbContext,
            new Mock<ILogger<CreateRoomCommandHandler>>().Object);
    }

    [Fact]
    public async Task HandleShouldCreateRoom()
    {
        // Arrange
        var command = new CreateRoomCommand
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };

        // Act
        var result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.Room.ShouldNotBeNull();
        result.Room.Id.ShouldBeGreaterThan(0);
        result.Room.Name.ShouldBe(command.Name);
        result.Room.Type.ShouldBe(command.Type);
        result.Room.Price.ShouldBe(command.Price);

    }
}