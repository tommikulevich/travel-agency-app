using MassTransit;
using Microsoft.EntityFrameworkCore;
using HotelService.Data;
using HotelService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x => {
    x.AddConsumer<AvailableRoomsConsumer>();
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host("rabbitmq", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });
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