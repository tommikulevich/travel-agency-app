using ApiGateway.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Payment.Events;
using Shared.Trip.Dtos;
using Shared.Trip.Events;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        readonly IRequestClient<ProcessPaymentFromCustomerEvent> _ProcessPaymentFromCustomerEvent;

        public PaymentController(IRequestClient<ProcessPaymentFromCustomerEvent> ProcessPaymentFromCustomerEvent)
        {
            _ProcessPaymentFromCustomerEvent = ProcessPaymentFromCustomerEvent;
        }
        [HttpPost]
        [Route("Payment")]
        public async Task<ProcessPaymentCustomerReplyEvent> Payment(Guid OfferId, double Price)
        {
            var response = await _ProcessPaymentFromCustomerEvent.GetResponse<ProcessPaymentCustomerReplyEvent>(new ProcessPaymentFromCustomerEvent() { CorrelationId = OfferId, Price = Price});
            return response.Message;
        }

    }

}
