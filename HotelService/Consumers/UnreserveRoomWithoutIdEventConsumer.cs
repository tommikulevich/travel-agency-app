using MassTransit;
using HotelService.Data;
using HotelService.Models;
using Shared.Hotel.Events;

namespace HotelService.Consumers 
{
    public class UnreserveRoomWithoutIdEventConsumer : IConsumer<UnreserveRoomWithoutIdEvent> 
    {
        private readonly HotelDbContext _context;

        public UnreserveRoomWithoutIdEventConsumer(HotelDbContext context) 
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UnreserveRoomWithoutIdEvent> context) 
        {
            Console.WriteLine($"Received unreservation request (without room ID) for Client ID: {context.Message.ClientId}");

            var rooms = _context.Room.Where(
                re => re.HotelId == context.Message.HotelId &&
                      re.NumOfPeople == context.Message.NumOfPeople &&
                      re.RoomType == context.Message.RoomType &&
                      re.Features == context.Message.Features).ToList();

            if (rooms.Count != 1) 
            {
                Console.WriteLine("UWAGA: niejednoznaczny wybór pokoi do cofnięcia rezerwacji!");
            }

            Room unreservedRoom = rooms.FirstOrDefault();

            var roomEvents = _context.RoomEvent.Where(
                re => re.RoomId == unreservedRoom.Id &&
                      re.StartDate == context.Message.ArrivalDate.ToUniversalTime() &&
                      re.EndDate == context.Message.ReturnDate.ToUniversalTime() &&
                      re.Status == "Reserved").ToList();

            if (!roomEvents.Any()) 
            {
                Console.WriteLine("No active reservations found to unreserve.");
                return;
            }
            
            _context.RoomEvent.RemoveRange(roomEvents);

            try 
            {
                _context.SaveChanges();
                Console.WriteLine($"Unreservation successful for Client ID: {context.Message.ClientId}");
                await context.Publish(new RoomsAvailabilityAfterUnreservationEvent {
                    Id = Guid.NewGuid(), 
                    CorrelationId = context.Message.CorrelationId
                });
                
                // await context.RespondAsync(new RoomsAvailabilityAfterUnreservationEvent {
                //     Id = Guid.NewGuid(), 
                //     CorrelationId = context.Message.CorrelationId
                // });
            } 
            catch (Exception e) 
            {
                Console.WriteLine($"Unreservation failed due to an error: {e.Message}");
            }
        }
    }
}