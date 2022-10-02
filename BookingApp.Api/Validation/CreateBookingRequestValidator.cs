using BookingApp.Contracts.Http;

using FluentValidation;

namespace BookingApp.Api.Validation;

internal class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.FirstName).Length(1, 255).NotNull();
        RuleFor(x => x.LastName).Length(1, 255).NotNull();
        RuleFor(x => x.PhoneNumber).Length(1, 15).NotNull();
        RuleFor(x => x.RoomId).NotNull();
        RuleFor(x => x.CheckInDate).NotNull();
        RuleFor(x => x.CheckOutDate).NotNull();
        RuleFor(x => x.Comment).Length(0, 500);
    }
}