using MassTransit;
using HotelService.Data;
using Shared.Hotel.Events;

namespace HotelService.Consumers {
    public class UnreserveRoomEventConsumer : IConsumer<UnreserveRoomEvent> {
        private readonly HotelDbContext _context;

        public UnreserveRoomEventConsumer(HotelDbContext context) {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UnreserveRoomEvent> context) {
            Console.WriteLine($"Received unreservation request for Client ID: {context.Message.ClientId}");

            var roomEvents = _context.RoomEvent.Where(
                re => re.StartDate == context.Message.ArrivalDate &&
                      re.EndDate == context.Message.ReturnDate &&
                      re.Status == "Reserved").ToList();

            if (!roomEvents.Any()) {
                Console.WriteLine("No active reservations found to unreserve.");
                await context.Publish(new UnreserveRoomReplyEvent {
                    Id = Guid.NewGuid(), // We can replace with context.Message.Id if necessary
                    CorrelationId = context.Message.CorrelationId,
                    Success = false
                });
                return;
            }
            
            _context.RoomEvent.RemoveRange(roomEvents);

            try {
                _context.SaveChanges();
                Console.WriteLine($"Unreservation successful for Client ID: {context.Message.ClientId}");
                await context.RespondAsync(new UnreserveRoomReplyEvent {
                    Id = Guid.NewGuid(), // We can replace with context.Message.Id if necessary
                    CorrelationId = context.Message.CorrelationId,
                    Success = true
                });
            } catch (Exception e) {
                Console.WriteLine($"Unreservation failed due to an error: {e.Message}");
                await context.RespondAsync(new UnreserveRoomReplyEvent {
                    Id = Guid.NewGuid(), 
                    CorrelationId = context.Message.CorrelationId,
                    Success = false
                });
            }
        }
    }
}