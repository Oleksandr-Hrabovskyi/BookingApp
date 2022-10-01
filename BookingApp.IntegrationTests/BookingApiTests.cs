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
    public async Task CreateBookingShouldReturnBookingId()
    {
        // Arrange
        var RoomId = new Random();
        var request = new CreateBookingRequest
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            PhoneNumber = "+380991234567",
            RoomId = RoomId.Next(1, 100),
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
}