using System;

using BookingApp.Domain.Database;
using BookingApp.UnitTests.Helpers;

using MediatR;

namespace BookingApp.UnitTests.Base;

public abstract class BaseHandlerTest<TRequest, TResult> : IDisposable
    where TRequest : IRequest<TResult>
{
    protected readonly BookingDbContext DbContext;
    protected readonly IRequestHandler<TRequest, TResult> Handler;
    public BaseHandlerTest()
    {
        DbContext = DbContextHelper.CreateTestDb();
        Handler = CreateHandler();
    }

    protected abstract IRequestHandler<TRequest, TResult> CreateHandler();

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}