using Microsoft.EntityFrameworkCore;
using MassTransit;
using PaymentService.Consumers;
using PaymentService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<ProcessPaymentEvent>();

    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {
        rabbitCfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        rabbitCfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.Run();