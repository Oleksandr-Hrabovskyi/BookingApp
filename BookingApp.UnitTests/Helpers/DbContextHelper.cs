using System.IO;

using BookingApp.Domain.Database;

using Microsoft.EntityFrameworkCore;

namespace BookingApp.UnitTests.Helpers;

internal static class DbContextHelper
{
    public static BookingDbContext CreateTestDb()
    {
        var tempFile = Path.GetTempFileName();
        return CreateTestDb($"Data Source={tempFile};");
    }

    public static BookingDbContext CreateTestDb(string connectionString)
    {
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseSqlite(connectionString)
            .Options;

        var dbContext = new BookingDbContext(options);
        dbContext.Database.Migrate();

        return dbContext;
    }
}