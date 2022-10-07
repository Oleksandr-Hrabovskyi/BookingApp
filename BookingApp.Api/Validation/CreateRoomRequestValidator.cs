using BookingApp.Contracts.Http;

using FluentValidation;

namespace BookingApp.Api.Validation;

internal class CreateRoomRequestValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomRequestValidator()
    {
        RuleFor(x => x.Name).Length(1, 100).NotNull();
        RuleFor(x => x.Type).Length(1, 100).NotNull();
        RuleFor(x => x.Price).NotNull();
    }
}