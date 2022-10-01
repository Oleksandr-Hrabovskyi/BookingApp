using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;
using BookingApp.Domain.Database;
using BookingApp.Domain.Exceptions;
using BookingApp.Domain.Queries;
using BookingApp.UnitTests.Helpers;

using MediatR;

using Shouldly;

namespace BookingApp.UnitTests.Queries;

public class BookingQueryHandlerTests : IDisposable
{
    private readonly BookingDbContext _dbContext;
    private readonly IRequestHandler<BookingQuery, BookingQueryResult> _handler;
    public BookingQueryHandlerTests()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new BookingQueryHandler(_dbContext);
    }

    [Fact]
    public async Task HandleShouldReturnBooking()
    {
        // Arrange
        var dbContext = DbContextHelper.CreateTestDb();
        var RoomId = new Random();
        var booking = new Booking
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = Guid.NewGuid().ToString(),
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21),
            RoomId = RoomId.Next(0, 100),
            Comment = Guid.NewGuid().ToString()
        };

        await dbContext.AddAsync(booking);
        await dbContext.SaveChangesAsync();

        var query = new BookingQuery
        {
            BookingId = booking.Id
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

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
            await _handler.Handle(query, CancellationToken.None);
        }
        catch (BookingException be) when (be.ErrorCode == ErrorCode.BookingNotFound
            && be.Message == $"Booking {bookingId} not found")
        {
            // Arrange
            // ignore
        }
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}