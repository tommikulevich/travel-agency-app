using MassTransit;
using PaymentService.Consumers;

var builder = WebApplication.CreateBuilder(args);

string rabbitmqHost = builder.Configuration["RABBITMQ_HOST"] ?? "rabbitmq";
string rabbitmqPort = builder.Configuration["RABBITMQ_PORT"] ?? "5672";

Console.WriteLine("RabbitMQ host: ", rabbitmqHost);
Console.WriteLine("RabbitMQ port: ", rabbitmqPort);

#pragma warning disable CS0618 // Disable the obsolete warning (UseInMemoryOutbox())
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<ProcessPaymentEvent>(context =>
    {
        context.UseMessageRetry(r => r.Interval(3, 1000));
        context.UseInMemoryOutbox();
    });
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
#pragma warning restore CS0618 // Re-enable the obsolete warning

var app = builder.Build();

app.Run();