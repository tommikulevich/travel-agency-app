using MassTransit;
using Microsoft.EntityFrameworkCore;
using TripService.Data;
using TripService.Consumers;
using TripService.Saga;

var builder = WebApplication.CreateBuilder(args);

string dbConn = builder.Configuration["DATABASE_CONNECTION_STRING"] ??
    "Host=host.docker.internal;Port=5432;Database=tripdb;Username=postgres;Password=guest;";
string rabbitmqHost = builder.Configuration["RABBITMQ_HOST"] ?? "rabbitmq";
string rabbitmqPort = builder.Configuration["RABBITMQ_PORT"] ?? "5672";

Console.WriteLine("Database connection string: ", dbConn);
Console.WriteLine("RabbitMQ host: ", rabbitmqHost);
Console.WriteLine("RabbitMQ port: ", rabbitmqPort);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(dbConn));
builder.Services.AddScoped<ITripRepo, TripRepo>();

#pragma warning disable CS0618 // Disable the obsolete warning (UseInMemoryOutbox())
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<GetAllTripsConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<GetTripByUserIdConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<GetTripsByPreferencesConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<GetAllPreferencesConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<ChangeRoomsAvailabilityStatusConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<ChangeSeatsAvailabilityStatusConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<ChangeReservationStatusConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<UnchangeRoomsAvailabilityStatusConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<CheckReservationStatusConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<GenerateChangesConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddSagaStateMachine<ReservationStateMachine, ReservationState>().InMemoryRepository();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Init own database
// PrepDb.PrepPopulation(app);

app.Run();
