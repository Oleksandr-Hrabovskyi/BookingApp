using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using BookingApp.Contracts.Http;

using Microsoft.AspNetCore.Mvc.Testing;

using Shouldly;

namespace BookingApp.IntegrationTests;

public class BookingApiTests
{
    private readonly HttpClient _client;

    public BookingApiTests()
    {
        var application = new WebApplicationFactory<Program>();
        _client = application.CreateClient();
    }

    [Fact]
    public async Task CreateBookingShouldReturnNotFoundIfNoRoom()
    {
        // Arrange
        var roomId = -1;
        var request = new CreateBookingRequest
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = Guid.NewGuid().ToString(),
            RoomId = roomId,
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21),
            Comment = Guid.NewGuid().ToString()
        };

        // Act
        using var response = await _client.PutAsJsonAsync("api/booking", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.RoomNotFound);
        result.Message.ShouldBe($"Room {roomId} not found");
    }

    [Fact]
    public async Task CreateBookingShouldReturnBookingId()
    {
        // Arrange
        var createRoomRequest = new CreateRoomRequest
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };

        using var rommResponse = await _client.PutAsJsonAsync("api/room", createRoomRequest);

        var roomResult = await rommResponse.Content.ReadFromJsonAsync<CreateRoomResponse>();

        var request = new CreateBookingRequest
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = Guid.NewGuid().ToString(),
            RoomId = roomResult.Id,
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21),
            Comment = Guid.NewGuid().ToString()
        };

        // Act
        using var response = await _client.PutAsJsonAsync("api/booking", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateBookingResponse>();
        result.Id.ShouldBeGreaterThan(0);
    }

    [Theory]
    [InlineData(null, "abcd")]
    [InlineData("s8KgXCD1R1fMwrIUYATxhxTaBECEkuXQiOgF1D2Sez59npgN6q2zvxKd6699sP4TPBuPnSXqSMny402ftsyNWYIXQlbJ0KKQA9jUNH0BLAKzmkXEbs4FFib7rPO7CN4PaYeWoWVXyVdYY76iqJCKwp0Er1q6pmHSUAei6atUUdvD3mGjE2ZtxEUKx2ZmvolUXvub8vthKODzEU777gIlLZSuacMfGTqPV6GYm4lHjrqC3hYOGDMdV7NsFGmAEA13", "abcd")]
    [InlineData("abcd", "s8KgXCD1R1fMwrIUYATxhxTaBECEkuXQiOgF1D2Sez59npgN6q2zvxKd6699sP4TPBuPnSXqSMny402ftsyNWYIXQlbJ0KKQA9jUNH0BLAKzmkXEbs4FFib7rPO7CN4PaYeWoWVXyVdYY76iqJCKwp0Er1q6pmHSUAei6atUUdvD3mGjE2ZtxEUKx2ZmvolUXvub8vthKODzEU777gIlLZSuacMfGTqPV6GYm4lHjrqC3hYOGDMdV7NsFGmAEA13")]
    public async Task CreateBookingShouldReturnBadRequest(string firstName, string lastName)
    {
        // Arrange
        var createRoomRequest = new CreateRoomRequest
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };

        using var rommResponse = await _client.PutAsJsonAsync("api/room", createRoomRequest);

        var roomResult = await rommResponse.Content.ReadFromJsonAsync<CreateRoomResponse>();

        var request = new CreateBookingRequest
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = "+380991234567",
            RoomId = roomResult.Id,
            CheckInDate = new DateTime(2022, 9, 20),
            CheckOutDate = new DateTime(2022, 9, 21),
            Comment = Guid.NewGuid().ToString()
        };

        // Act
        using var response = await _client.PutAsJsonAsync("api/booking", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.BadRequest);
        result.Message.ShouldBe("Invalid request");
    }
}