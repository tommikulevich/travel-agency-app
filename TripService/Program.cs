using MassTransit;
using Microsoft.EntityFrameworkCore;
using TripService.Data;
using TripService.Consumers;
using MassTransit.Futures.Contracts;
using TripService.Saga;
// using TripService.Sagas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration["DATABASE_CONNECTION_STRING"]));
// builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<ITripRepo, TripRepo>();
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
    // cfg.AddConsumer<SaveTripConsumer>(context =>
    // {
    //     context.UseMessageRetry(r => r.Interval(3, 1000));
    //     context.UseInMemoryOutbox();
    // });
    // cfg.AddConsumer<CreateTripConsumer>(context =>
    // {
    //     context.UseMessageRetry(r => r.Interval(3, 1000));
    //     context.UseInMemoryOutbox();
    // });
    cfg.AddSagaStateMachine<ReservationStateMachine, ReservationState>().InMemoryRepository();
    cfg.AddDelayedMessageScheduler();
    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {
        rabbitCfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        rabbitCfg.UseDelayedMessageScheduler();
        rabbitCfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// init database
//PrepDb.PrepPopulation(app);



app.Run();


// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
