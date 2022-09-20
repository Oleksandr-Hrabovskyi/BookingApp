using BookingApp.Contracts.Database;

using Microsoft.EntityFrameworkCore;

namespace BookingApp.Domain.Database;

public class BookingDbContext : DbContext
{
    public DbSet<Room> Room { get; init; }
    public DbSet<Booking> Booking { get; init; }
    public BookingDbContext() : base()
    {
    }
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {
    }
}
