using ApiGateway.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration["DATABASE_CONNECTION_STRING"]));
builder.Services.AddScoped<IUserRepo, UserRepo>();


string rabbitmqHost = builder.Configuration["RABBITMQ_HOST"];
        string rabbitmqHostPortString = builder.Configuration["RABBITMQ_PORT"];

        if (!int.TryParse(rabbitmqHostPortString, out int rabbitmqPort))
        {
            throw new InvalidOperationException("Invalid port number in configuration");
        }

builder.Services.AddMassTransit(cfg =>
{
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapControllers();

app.Run();

