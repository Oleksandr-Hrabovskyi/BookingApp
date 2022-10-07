using System.Collections.Generic;

using BookingApp.Contracts.Database;

namespace BookingApp.Contracts.Http;

public class GetAllRoomQueryResult
{
    public List<Room> Rooms { get; init; }
}