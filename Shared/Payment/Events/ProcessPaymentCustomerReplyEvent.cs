using System.ComponentModel.DataAnnotations;
// using Shared.PaymentService.Events;

namespace Shared.Payment.Events
{
    public class ProcessPaymentCustomerReplyEvent
    {
        public const string ACCEPTED = "ACCEPTED";
        public const string INVALID_CARD = "INVALID CARD";
        public const string LIMITS = "TOO LOW LIMITS";

        public string Response {get; set;}
        public Guid Id {get; set;}
        public Guid CorrelationId {get; set;}

        public ProcessPaymentCustomerReplyEvent(string response, Guid correlationId) : base()
        {
            Response = response;
            CorrelationId = correlationId;
        }

        public ProcessPaymentCustomerReplyEvent() {}
    }
}