using MassTransit;
using Shared.Payment.Events;

namespace PaymentService.Consumers
{
    public class ProcessPaymentEvent : IConsumer<ProcessPaymentFromCustomerEvent>
    {
        private Random random = new();
        public async Task Consume(ConsumeContext<ProcessPaymentFromCustomerEvent> context)
        {
            var price = context.Message.Price;
            var OfferId = context.Message.CorrelationId;
            Console.WriteLine("Payment in service");
            await Task.Delay(1500);
            int randomNumber = random.Next(0, 10);
            bool result;
            if (randomNumber < 1)
            {
                Console.WriteLine("Payment rejected!");
                result = false;
            }
            else
            {
                Console.WriteLine("Payment accepted!");
                result = true;
            }
            await context.RespondAsync<ProcessPaymentCustomerReplyEvent>(new ProcessPaymentCustomerReplyEvent() {CorrelationId = OfferId, result = result});
            await context.Publish(new ProcessPaymentCustomerReplyEvent() {CorrelationId = OfferId, result = result});

        }
    }
}