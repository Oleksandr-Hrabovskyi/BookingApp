using BookingApp.Contracts.Http;

using FluentValidation;

namespace BookingApp.Api.Validation;

internal class DeleteRoomRequestValidator : AbstractValidator<DeleteRoomRequest>
{
    public DeleteRoomRequestValidator()
    {
        RuleFor(x => x.RoomId).NotNull();
    }
}