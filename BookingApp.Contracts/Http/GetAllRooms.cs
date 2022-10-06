using System.Collections.Generic;

using BookingApp.Contracts.Database;

namespace BookingApp.Contracts.Http;

public class GetAllRoomsQueryResult
{
    public List<Room> Rooms { get; init; }
}