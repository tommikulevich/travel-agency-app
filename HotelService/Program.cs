using MassTransit;
using Microsoft.EntityFrameworkCore;
using HotelService.Data;
using HotelService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMassTransit(x => {
    x.AddConsumer<AvailableRoomsConsumer>();
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("hotel-queue", e => {
            e.ConfigureConsumer<AvailableRoomsConsumer>(context);
        });
    });
});

// builder.Services.AddDbContext<HotelDbContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("HotelDatabase")));
builder.Services.AddDbContext<HotelDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<IHotelRepo, HotelRepo>();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();