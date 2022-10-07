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

public class GetAllBookingsQueryHandlerTests : BaseHandlerTest<GetAllBookingsQuery, GetAllBookingsQueryResult>
{
    public GetAllBookingsQueryHandlerTests() : base()
    {
    }

    protected override IRequestHandler<GetAllBookingsQuery, GetAllBookingsQueryResult> CreateHandler()
    {
        return new GetAllBookingsQueryHandler(DbContext,
            new Mock<ILogger<GetAllBookingsQueryHandler>>().Object);
    }

    [Fact]
    public async Task HandleShouldReturnAllBooking()
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

        var booking1 = new Booking
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = Guid.NewGuid().ToString(),
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21),
            RoomId = room1.Id,
            Comment = Guid.NewGuid().ToString()
        };
        await DbContext.AddAsync(booking1);
        await DbContext.SaveChangesAsync();

        var booking2 = new Booking
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = Guid.NewGuid().ToString(),
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21),
            RoomId = room2.Id,
            Comment = Guid.NewGuid().ToString()
        };
        await DbContext.AddAsync(booking2);
        await DbContext.SaveChangesAsync();

        var bookings = new List<Booking>()
        {
            booking1,
            booking2
        };

        var query = new GetAllBookingsQuery
        {
            BookingId = booking1.Id
        };

        // Act
        var result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Bookings.ShouldNotBeNull();

    }

    [Fact]
    public async Task HandleShouldThrowExceptionIfNoAllBooking()
    {
        // Arrange
        var bookingId = -1;
        var query = new GetAllBookingsQuery
        {
            BookingId = bookingId
        };

        try
        {
            // Act
            await Handler.Handle(query, CancellationToken.None);
        }
        catch (BookingException be) when (be.ErrorCode == ErrorCode.BookingNotFound
            && be.Message == $"Bookings {bookingId} not found")
        {
            // Assert
            // ignore
        }
    }
}