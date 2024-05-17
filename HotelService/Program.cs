using MassTransit;
using Microsoft.EntityFrameworkCore;
using HotelService.Data;
using HotelService.Consumers;

var builder = WebApplication.CreateBuilder(args);

string dbConn = builder.Configuration["DATABASE_CONNECTION_STRING"] ??
    "Host=host.docker.internal;Port=5432;Database=hoteldb;Username=postgres;Password=guest;";
string rabbitmqHost = builder.Configuration["RABBITMQ_HOST"] ?? "rabbitmq";
string rabbitmqPort = builder.Configuration["RABBITMQ_PORT"] ?? "5672";

Console.WriteLine("Database connection string: ", dbConn);
Console.WriteLine("RabbitMQ host: ", rabbitmqHost);
Console.WriteLine("RabbitMQ port: ", rabbitmqPort);

#pragma warning disable CS0618 // Disable the obsolete warning (UseInMemoryOutbox())
builder.Services.AddMassTransit(x => {
    x.AddConsumer<AvailableRoomsConsumer>();
    x.AddConsumer<ReserveRoomEventConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    x.AddConsumer<UnreserveRoomEventConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    x.AddDelayedMessageScheduler();
    x.UsingRabbitMq((context, rabbitCfg) =>
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

builder.Services.AddDbContext<HotelDbContext>(options => options.UseNpgsql(dbConn));
builder.Services.AddScoped<IHotelRepo, HotelRepo>();

var app = builder.Build();

app.Run();