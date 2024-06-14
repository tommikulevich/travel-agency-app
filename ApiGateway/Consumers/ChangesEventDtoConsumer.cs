using Microsoft.AspNetCore.SignalR;
using MassTransit;
using Shared.Trip.Dtos;

namespace ApiGateway.Consumers
{
    public class ChangesEventDtoConsumer : IConsumer<ChangesEventDto>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public ChangesEventDtoConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<ChangesEventDto> context)
        {
            Console.WriteLine("Change in offer was generated");
            
            Guid OfferId = context.Message.OfferId;
            var changedType = context.Message.ChangeType;
            var changedValue = context.Message.ChangeValue;
            var previousValue = context.Message.PreviousValue;

            await _hubContext.Clients.All.SendAsync("Change", $"{OfferId};{changedType};{changedValue};{previousValue}");
            await _hubContext.Clients.All.SendAsync("ChangedOffer", $"{OfferId};{changedType};{changedValue};{previousValue}");
            
            await Task.Yield();     // Ensures that method runs asynchronously and avoids the warning
        }
    }
}