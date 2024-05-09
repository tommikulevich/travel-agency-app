using System.ComponentModel.DataAnnotations;
// using Shared.PaymentService.Events;

namespace Shared.Payment.Events
{
    public class ProcessPaymentFromCustomerEvent
    {
        public CardCredentials Card {get; set;}
        public double Price {get; set;}
        public Guid Id {get; set;}
        public Guid CorrelationID {get; set;}
        public ProcessPaymentFromCustomerEvent(CardCredentials card, double price)
        {
            Card = card;
            Price = price;
        }

        public ProcessPaymentFromCustomerEvent(){}
    }


    public class CardCredentials
    {
        public string CardNumber {get; set;}
        public int CVV {get; set;}
        public string ExpirationDate {get; set;}
        public string FullName {get; set;}
    }
}