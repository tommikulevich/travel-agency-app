using System;
using System.Threading.Tasks;
using FlightService.Repo;
using FlightService.Data;
using Flight.Consumers;
using Shared.Flight.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FlightContext>(options =>
    options.UseNpgsql(builder.Configuration["DATABASE_CONNECTION_STRING"]));
builder.Services.AddScoped<IFlightRepo, FlightRepo>();

string rabbitmqHost = builder.Configuration["RABBITMQ_HOST"];
        string rabbitmqHostPortString = builder.Configuration["RABBITMQ_PORT"];

        if (!int.TryParse(rabbitmqHostPortString, out int rabbitmqPort))
        {
            throw new InvalidOperationException("Invalid port number in configuration");
        }

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
    //cfg.AddSagaStateMachine<ReservationStateMachine, StatefulReservation>().InMemoryRepository();
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
var app = builder.Build();
using (var contScope = app.Services.CreateScope())
using (var context = contScope.ServiceProvider.GetRequiredService<FlightContext>())
{
    // Ensure Deleted possible to use for testing
    context.Database.EnsureCreated();
    context.SaveChanges(); // save to DB
    Console.WriteLine("Done clearing database");
}
app.Run();









