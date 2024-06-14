using Microsoft.EntityFrameworkCore;
using MassTransit;
using ApiGateway.Data;
using ApiGateway.Consumers;
using ApiGateway.Singletons;

var builder = WebApplication.CreateBuilder(args);

string dbConn = builder.Configuration["DATABASE_CONNECTION_STRING"] ??
    "Host=host.docker.internal;Port=5432;Database=userdb;Username=postgres;Password=guest;";
string rabbitmqHost = builder.Configuration["RABBITMQ_HOST"] ?? "rabbitmq";
string rabbitmqPort = builder.Configuration["RABBITMQ_PORT"] ?? "5672";

Console.WriteLine("Database connection string: ", dbConn);
Console.WriteLine("RabbitMQ host: ", rabbitmqHost);
Console.WriteLine("RabbitMQ port: ", rabbitmqPort);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(x => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(dbConn));
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddSingleton<GenerationState>();

#pragma warning disable CS0618 // Disable the obsolete warning (UseInMemoryOutbox())
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<NewDestinationPreferenceConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.AddConsumer<ChangesEventDtoConsumer>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {
        rabbitCfg.Host(new Uri($"rabbitmq://{rabbitmqHost}:{rabbitmqPort}/"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        rabbitCfg.ConfigureEndpoints(context);
    });
});
#pragma warning restore CS0618 // Re-enable the obsolete warning

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.MapControllers();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
