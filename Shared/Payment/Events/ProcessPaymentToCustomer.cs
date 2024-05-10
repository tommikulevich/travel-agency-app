using System.ComponentModel.DataAnnotations;
// using Shared.PaymentService.Events;

namespace Shared.Payment.Events
{
    public class ProcessPaymentReplyCustomerEvent
    {
        public const string ACCEPTED = "ACCEPTED";
        public const string INVALID_CARD = "INVALID CARD";
        public const string LIMITS = "TOO LOW LIMITS";

        public string Response {get; set;}
        public Guid Id {get; set;}
        public Guid CorrelationID {get; set;}

        public ProcessPaymentReplyCustomerEvent(string response, Guid correlationId) : base()
        {
            Response = response;
            CorrelationID = correlationId;
        }

        public ProcessPaymentReplyCustomerEvent() {}
    }
}