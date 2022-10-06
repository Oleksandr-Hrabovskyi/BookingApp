using System.Threading.Tasks;

using BookingApp.Contracts.Database;

namespace BookingApp.Domain.Helpers.Interfaces;

public interface IValidationBooking
{
    Task<bool> BookingValidate(Booking booking);
}