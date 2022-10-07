using BookingApp.Contracts.Http;

using FluentValidation;

namespace BookingApp.Api.Validation;

internal class DeleteBookingRequestValidator : AbstractValidator<DeleteBookingRequest>
{
    public DeleteBookingRequestValidator()
    {
        RuleFor(x => x.BookingId).NotNull();
    }
}