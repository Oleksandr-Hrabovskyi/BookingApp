using System.IO;

using BookingApp.Domain.Database;

using Microsoft.EntityFrameworkCore;

namespace BookingApp.UnitTests.Helpers;

internal static class DbContextHelper
{
    public static BookingDbContext CreateTestDb()
    {
        var tempfile = Path.GetTempFileName();
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseSqlite("Data Source={tempfile};")
            .Options;

        var dbContext = new BookingDbContext(options);
        dbContext.Database.Migrate();

        return dbContext;
    }
}