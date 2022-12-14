namespace BookingApp.Contracts.Http;

public enum ErrorCode
{
    BadRequest = 40000,
    BookingNotFound = 40401,
    RoomNotFound = 40402,
    InternalServerError = 50000,
    DbFailureError = 50001
}

public class ErrorResponse
{
    public ErrorCode Code { get; init; }
    public string Message { get; init; }
}