using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using BookingApp.Contracts.Database;
using BookingApp.Contracts.Http;

using Microsoft.AspNetCore.Mvc.Testing;

using Shouldly;

namespace BookingApp.IntegrationTests;

public class RoomApiTests
{
    private readonly HttpClient _client;

    public RoomApiTests()
    {
        var application = new WebApplicationFactory<Program>();
        _client = application.CreateClient();
    }

    [Fact]
    public async Task CreateRoomShouldReturnRoomId()
    {
        // Arrange
        var request = new Room
        {
            Name = Guid.NewGuid().ToString(),
            Type = Guid.NewGuid().ToString(),
            Price = new Random().Next(1000, 2500)
        };

        // Act
        using var response = await _client.PutAsJsonAsync("api/room", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateRoomResponse>();
        result.Id.ShouldBeGreaterThan(0);
    }
}