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
                re => re.RoomId == context.Message.RoomId &&
                      re.StartDate == context.Message.ArrivalDate.ToUniversalTime() &&
                      re.EndDate == context.Message.ReturnDate.ToUniversalTime() &&
                      re.Status == "Reserved").ToList();

            if (!roomEvents.Any()) {
                Console.WriteLine("No active reservations found to unreserve.");
                return;
            }
            
            _context.RoomEvent.RemoveRange(roomEvents);

            try {
                _context.SaveChanges();
                Console.WriteLine($"Unreservation successful for Client ID: {context.Message.ClientId}");
                
                await context.RespondAsync(new RoomsAvailabilityAfterUnreservationEvent {
                    Id = Guid.NewGuid(), 
                    CorrelationId = context.Message.CorrelationId
                });
            } catch (Exception e) {
                Console.WriteLine($"Unreservation failed due to an error: {e.Message}");
            }
        }
    }
}