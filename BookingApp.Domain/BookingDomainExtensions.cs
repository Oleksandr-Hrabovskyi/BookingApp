using System;

using BookingApp.Domain.Commands;
using BookingApp.Domain.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.Domain;

public static class BookingDomainExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> dbOptionsAction)
    {
        return services.AddMediatR(typeof(CreateBookingCommand))
        .AddDbContext<BookingDbContext>(dbOptionsAction);
    }
}