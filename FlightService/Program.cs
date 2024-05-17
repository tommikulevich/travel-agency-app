using MassTransit;
using Microsoft.EntityFrameworkCore;
using FlightService.Repo;
using FlightService.Data;
using Flight.Consumers;

var builder = WebApplication.CreateBuilder(args);

string dbConn = builder.Configuration["DATABASE_CONNECTION_STRING"] ??
    "Host=host.docker.internal;Port=5432;Database=flightdb;Username=postgres;Password=guest;";
string rabbitmqHost = builder.Configuration["RABBITMQ_HOST"] ?? "rabbitmq";
string rabbitmqPort = builder.Configuration["RABBITMQ_PORT"] ?? "5672";

Console.WriteLine("Database connection string: ", dbConn);
Console.WriteLine("RabbitMQ host: ", rabbitmqHost);
Console.WriteLine("RabbitMQ port: ", rabbitmqPort);

builder.Services.AddDbContext<FlightContext>(options => options.UseNpgsql(dbConn));
builder.Services.AddScoped<IFlightRepo, FlightRepo>();

#pragma warning disable CS0618 // Disable the obsolete warning (UseInMemoryOutbox())
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<GetAvailableFlightsEventConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<ReserveSeatsEventConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });

    cfg.AddDelayedMessageScheduler();
    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {
        rabbitCfg.Host(new Uri($"rabbitmq://{rabbitmqHost}:{rabbitmqPort}/"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        rabbitCfg.UseDelayedMessageScheduler();
        rabbitCfg.ConfigureEndpoints(context);
    });
});
#pragma warning restore CS0618 // Re-enable the obsolete warning

var app = builder.Build();

app.Run();
