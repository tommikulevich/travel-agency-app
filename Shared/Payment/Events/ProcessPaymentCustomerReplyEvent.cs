namespace Shared.Payment.Events
{
    public class ProcessPaymentCustomerReplyEvent
    {
        public Guid Id {get; set;}
        public Guid CorrelationId {get; set;}
        public bool result {get; set;}
    }
}