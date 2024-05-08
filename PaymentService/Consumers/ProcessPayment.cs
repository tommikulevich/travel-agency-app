using MassTransit;
using PaymentService.Models;

namespace PaymentService.Consumers
{
    public class ProcessPaymentEvent : IConsumer<ProcessPaymentFromCustomerEvent>
    {
        private Random random = new();
        public async Task Consume(ConsumeContext<ProcessPaymentFromCustomerEvent> context)
        {
            var @paymentEvent = context.Message;
            var res = await ProcessPaymentFromCard(@paymentEvent.Card);
            if(res)
                await context.Publish(new ProcessPaymentReplyCustomerEvent(ProcessPaymentReplyCustomerEvent.ACCEPTED, @paymentEvent.CorrelationID));
            else
            {
                DateTime expiryDate;
                if (!DateTime.TryParse(paymentEvent.Card.ExpirationDate, out expiryDate))
                    await context.Publish(new ProcessPaymentReplyCustomerEvent(ProcessPaymentReplyCustomerEvent.INVALID_CARD, @paymentEvent.CorrelationID));
                if(random.Next(100) < 10 || expiryDate < DateTime.Now || @paymentEvent.Card.CVV % 10 > 5)
                    await context.Publish(new ProcessPaymentReplyCustomerEvent(ProcessPaymentReplyCustomerEvent.INVALID_CARD, @paymentEvent.CorrelationID));
                else
                    await context.Publish(new ProcessPaymentReplyCustomerEvent(ProcessPaymentReplyCustomerEvent.LIMITS, @paymentEvent.CorrelationID));
            }
        }

        public async Task<bool> ProcessPaymentFromCard(CardCredentials credentials)
        {
            await Task.Delay(2000);
            DateTime expiryDate;
            if (!DateTime.TryParse(credentials.ExpirationDate, out expiryDate))
                return false;
            if (expiryDate < DateTime.Now)
                return false;
            if (credentials.CVV == 420)
                return false;
            if (credentials.CardNumber.StartsWith("123456"))
                return false;
            await Task.Delay(2000);
            if (credentials.CardNumber.StartsWith("51"))
                return true;
            if (random.Next(100) > 31)
                return true;
            await Task.Delay(2000);
            if (random.Next(100) > 10)
                return true;
            return false;
        }
    }
}