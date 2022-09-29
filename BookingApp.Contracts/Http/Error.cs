namespace BookingApp.Contracts.Http;

public enum ErrorCode
{
    BookingNotFound = 40401,
    InternalServerError = 50000,
    DbFailureError = 50001
}

public class ErrorResponse
{
    public ErrorCode Code { get; init; }
    public string Message { get; init; }
}