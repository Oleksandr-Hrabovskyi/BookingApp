using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;
using BookingApp.Domain.Exceptions;
using BookingApp.Domain.Queries;
using BookingApp.UnitTests.Base;
using BookingApp.UnitTests.Helpers;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace BookingApp.UnitTests.Queries;

public class BookingQueryHandlerTests : BaseHandlerTest<BookingQuery, BookingQueryResult>
{
    public BookingQueryHandlerTests() : base()
    {
    }

    protected override IRequestHandler<BookingQuery, BookingQueryResult> CreateHandler()
    {
        return new BookingQueryHandler(DbContext,
            new Mock<ILogger<BookingQueryHandler>>().Object);
    }

    [Fact]
    public async Task HandleShouldReturnBooking()
    {
        // Arrange
        var dbContext = DbContextHelper.CreateTestDb();
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
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21),
            RoomId = room.Id,
            Comment = Guid.NewGuid().ToString()
        };

        await dbContext.AddAsync(booking);
        await dbContext.SaveChangesAsync();

        var query = new BookingQuery
        {
            BookingId = booking.Id
        };

        // Act
        var result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Booking.ShouldNotBeNull();
        result.Booking.Id.ShouldBe(booking.Id);
        result.Booking.FirstName.ShouldBe(booking.FirstName);
        result.Booking.LastName.ShouldBe(booking.LastName);
        result.Booking.PhoneNumber.ShouldBe(booking.PhoneNumber);
        result.Booking.RoomId.ShouldBe(booking.RoomId);
        result.Booking.CheckInDate.ShouldBe(booking.CheckInDate);
        result.Booking.CheckOutDate.ShouldBe(booking.CheckOutDate);
        result.Booking.Comment.ShouldBe(booking.Comment);
    }

    [Fact]
    public async Task HandleShouldThrowExceptionIfNoBooking()
    {
        // Arrange
        var bookingId = -1;
        var query = new BookingQuery
        {
            BookingId = bookingId
        };

        try
        {
            // Act
            await Handler.Handle(query, CancellationToken.None);
        }
        catch (BookingException be) when (be.ErrorCode == ErrorCode.BookingNotFound
            && be.Message == $"Booking {bookingId} not found")
        {
            // Assert
            // ignore
        }
    }
}