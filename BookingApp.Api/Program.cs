using System;
using System.IO;
using System.Reflection;

using BookingApp.Api.Configuration;
using BookingApp.Domain;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BookingApp API",
        Description = "WEB API to create bookings"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddHealthChecks()
    .AddNpgSql((sp) =>
    {
        var configuration = sp.GetRequiredService<IOptionsMonitor<AppConfiguration>>();
        return configuration.CurrentValue.ConnectionString;
    },
        timeout: TimeSpan.FromSeconds(2));

builder.Services.Configure<AppConfiguration>(builder.Configuration);

builder.Services.AddDomainServices((sp, options) =>
{
    var configuration = sp.GetRequiredService<IOptionsMonitor<AppConfiguration>>();
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

    options.UseNpgsql(configuration.CurrentValue.ConnectionString)
        .UseLoggerFactory(loggerFactory);
});

// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddScoped<IValidator<CreateBookingRequest>, CreateBookingRequestValidator>();
// builder.Services.AddScoped<IValidator<CreateRoomRequest>, CreateRoomRequestValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI((options) =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});
}

app.MapControllers();
app.MapHealthChecks("/Health");

app.Run();

public partial class Program { }
