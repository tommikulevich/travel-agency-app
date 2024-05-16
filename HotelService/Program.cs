using MassTransit;
using Microsoft.EntityFrameworkCore;
using HotelService.Data;
using HotelService.Consumers;

var builder = WebApplication.CreateBuilder(args);


string rabbitmqHost = builder.Configuration["RABBITMQ_HOST"];
        string rabbitmqHostPortString = builder.Configuration["RABBITMQ_PORT"];

        if (!int.TryParse(rabbitmqHostPortString, out int rabbitmqPort))
        {
            throw new InvalidOperationException("Invalid port number in configuration");
        }


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

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseNpgsql(builder.Configuration["DATABASE_CONNECTION_STRING"]));
builder.Services.AddScoped<IHotelRepo, HotelRepo>();

var app = builder.Build();

app.Run();