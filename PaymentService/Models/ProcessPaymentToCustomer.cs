using System.ComponentModel.DataAnnotations;
using PaymentService.Models;

namespace PaymentService.Models
{
    public class ProcessPaymentReplyCustomerEvent : EventModelClass
    {
        public const string ACCEPTED = "ACCEPTED";
        public const string INVALID_CARD = "INVALID CARD";
        public const string LIMITS = "TOO LOW LIMITS";

        public string Response {get; set;}

        public ProcessPaymentReplyCustomerEvent(string response, Guid correlationId) : base()
        {
            Response = response;
            CorrelationID = correlationId;
        }

        public ProcessPaymentReplyCustomerEvent() {}
    }
}