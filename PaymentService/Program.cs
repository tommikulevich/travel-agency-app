using Microsoft.EntityFrameworkCore;
using MassTransit;
using PaymentService.Consumers;
using Shared.Payment.Events;

var builder = WebApplication.CreateBuilder(args);


string rabbitmqHost = builder.Configuration["RABBITMQ_HOST"];
        string rabbitmqHostPortString = builder.Configuration["RABBITMQ_PORT"];

        if (!int.TryParse(rabbitmqHostPortString, out int rabbitmqPort))
        {
            throw new InvalidOperationException("Invalid port number in configuration");
        }

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<ProcessPaymentEvent>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });;
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

app.Run();