using BookingApp.Domain.Database;
using BookingApp.Domain.Queries;

using MediatR;

namespace BookingApp.UnitTests.Queries;

public class BookingQueryHandlerTests
{
    private readonly BookingDbContext _dbContext;

    public BookingQueryHandlerTests(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private readonly IRequestHandler<BookingQuery, BookingQueryResult> _handler;
    public BookingQueryHandlerTests()
    {
        _handler = new BookingQueryHandler();
    }
}