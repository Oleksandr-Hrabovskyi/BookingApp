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
    [InlineData("MmpaGdmtmAyH16FxrvcDlsr5yg4F2MjAyzqxgwkWS9rkYhIL0OXoxBBIN7pG2wZUISfUOvJwfLBO3vLDfFOMfSoI0kwWRGnuUbyhzNTXlSGrwjruFNB9DPMo9quhVLRenUN9Z9e6QiumsvsfGW6vOh39kqcB2YBb73qZdFElNXaa0KXXWDv30zZYwGBGYmKKt0u9mW6ZW8aHpzm7pqq97RGtDAJamjoWQVhow6zqVG2jTgoMGC3pxUf7h5DdTuLZ", "abcd")]
    [InlineData("abcd", "0scB2jGXCGikDkNZdkSUYRPsztngMi9GoxO05ujplnwXiXmd9R6scb9YHZDiFERd6W7PEVcaovadVA2LWT6uJLopq0t1vlQNsh8xByJlzMS3KjsR5IRQZj9DDyS65WuOWfzo5B6Ma68Esyw2NLQIWeDXnOt7GEQ1QX68IPf7xS2Xsg85jclNeRCB2Bgl1phZnwUHsikyIpR5j6ZFhCkngOjwDUZ2Xx3sTEYBqxVcPtNBU0dWGGmSBqpoJnO2DeCBj4")]
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
            PhoneNumber = Guid.NewGuid().ToString(),
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