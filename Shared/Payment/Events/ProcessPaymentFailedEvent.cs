namespace Shared.Payment.Events
{
    public class ProcessPaymentFailedEvent
    {
        public Guid Id {get; set;}
        public Guid CorrelationID {get; set;}
    }
}