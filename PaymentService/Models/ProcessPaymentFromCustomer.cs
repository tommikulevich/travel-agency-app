using System.ComponentModel.DataAnnotations;
using PaymentService.Models;


namespace PaymentService.Models
{
    public class ProcessPaymentFromCustomerEvent : EventModelClass
    {
        public CardCredentials Card {get; set;}
        public double Price {get; set;}

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