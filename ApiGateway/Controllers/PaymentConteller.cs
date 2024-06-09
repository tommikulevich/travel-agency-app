using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shared.Payment.Events;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        readonly IRequestClient<ProcessPaymentFromCustomerEvent> _processPaymentFromCustomerEvent;
        private readonly IHubContext<NotificationHub> _hubContext;

        public PaymentController(IRequestClient<ProcessPaymentFromCustomerEvent> processPaymentFromCustomerEvent,
                                IHubContext<NotificationHub> hubContext)
        {
            _processPaymentFromCustomerEvent = processPaymentFromCustomerEvent;
            _hubContext = hubContext;
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
            if (response.Message.result)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"{offerId}\n was just reserved");
            }
            
            return response.Message;
        }
    }
}
