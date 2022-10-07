using System;

using BookingApp.Contracts.Http;

namespace BookingApp.Domain.Exceptions;

public class BookingException : Exception
{
    public ErrorCode ErrorCode { get; }

    public BookingException(ErrorCode errorCode) : this(errorCode, null)
    {
    }

    public BookingException(ErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}