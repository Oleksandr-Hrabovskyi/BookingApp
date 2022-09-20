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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=postgres;Password=1234;Host=localhost;Port=5432;Database=bookingdb;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;");
    }
}
