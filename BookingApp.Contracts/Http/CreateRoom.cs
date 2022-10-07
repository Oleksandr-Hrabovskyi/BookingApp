namespace BookingApp.Contracts.Http;

public class CreateRoomRequest
{
    public string Name { get; init; }
    public string Type { get; init; }
    public decimal Price { get; init; }
}

public class CreateRoomResponse
{
    public int Id { get; init; }
}