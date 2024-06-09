using MassTransit;
using Shared.ApiGateway.Events;
using Microsoft.AspNetCore.SignalR;

namespace ApiGateway.Consumers
{

    public class NewDestinationPreferenceConsumer : IConsumer<NewDestinationPreferenceEvent>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NewDestinationPreferenceConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<NewDestinationPreferenceEvent> context)
        {
            Console.WriteLine("Reservation status changed");
            
            Guid? CorrelationId = context.Message.CorrelationId;
            var newPreference = context.Message.newPreference;

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"{newPreference}\n is new preference");
            
            await Task.Yield(); 

        }

            // await Task.Yield();     // Ensures that method runs asynchronously and avoids the warning
    }
}
    
