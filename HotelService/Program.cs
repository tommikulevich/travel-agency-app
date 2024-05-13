using MassTransit;
using Microsoft.EntityFrameworkCore;
using HotelService.Data;
using HotelService.Consumers;

var builder = WebApplication.CreateBuilder(args);

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
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host("rabbitmq", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.UseDelayedMessageScheduler();
        cfg.ConfigureEndpoints(context);
        // cfg.ReceiveEndpoint("hotel-queue", e => {
        //     e.ConfigureConsumer<AvailableRoomsConsumer>(context);
        // });
    });
    
});

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseNpgsql(builder.Configuration["DATABASE_CONNECTION_STRING"]));
builder.Services.AddScoped<IHotelRepo, HotelRepo>();

var app = builder.Build();

app.Run();