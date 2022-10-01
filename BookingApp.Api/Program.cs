using System;

using BookingApp.Api.Configuration;
using BookingApp.Domain.Commands;
using BookingApp.Domain.Database;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddNpgSql((sp) =>
    {
        var configuration = sp.GetRequiredService<IOptionsMonitor<AppConfiguration>>();
        return configuration.CurrentValue.ConnectionString;
    },
        timeout: TimeSpan.FromSeconds(2));

builder.Services.Configure<AppConfiguration>(builder.Configuration);

builder.Services.AddMediatR(typeof(CreateBookingCommand));
builder.Services.AddDbContext<BookingDbContext>((sp, options) =>
{
    var configuration = sp.GetRequiredService<IOptionsMonitor<AppConfiguration>>();
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    options.UseNpgsql(configuration.CurrentValue.ConnectionString)
        .UseLoggerFactory(loggerFactory);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHealthChecks("/Health");

app.Run();
