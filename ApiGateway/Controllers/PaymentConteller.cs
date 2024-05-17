using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Payment.Events;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        readonly IRequestClient<ProcessPaymentFromCustomerEvent> _processPaymentFromCustomerEvent;

        public PaymentController(IRequestClient<ProcessPaymentFromCustomerEvent> processPaymentFromCustomerEvent)
        {
            _processPaymentFromCustomerEvent = processPaymentFromCustomerEvent;
        }

        [HttpPost]
        [Route("Payment")]
        public async Task<ProcessPaymentCustomerReplyEvent> Payment(Guid offerId, double price)
        {
            var response = await _processPaymentFromCustomerEvent.GetResponse<ProcessPaymentCustomerReplyEvent>(
                new ProcessPaymentFromCustomerEvent 
                { 
                    CorrelationId = offerId, 
                    Price = price 
                });
            return response.Message;
        }
    }
}
