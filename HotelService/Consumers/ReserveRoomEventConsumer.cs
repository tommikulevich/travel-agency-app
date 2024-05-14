using MassTransit;
using HotelService.Data;
using HotelService.Models;
using Shared.Hotel.Events;

namespace HotelService.Consumers {
    public class ReserveRoomEventConsumer : IConsumer<ReserveRoomEvent> {
        private readonly HotelDbContext _context; 

        public ReserveRoomEventConsumer(HotelDbContext context) {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ReserveRoomEvent> context) {
            bool reserved = false;
            var eventMessage = context.Message;
            var totalGuests = eventMessage.NumOfAdults + eventMessage.NumOfKidsTo18 + eventMessage.NumOfKidsTo10 + eventMessage.NumOfKidsTo3;

            var suitableRooms = _context.Hotel
                .Where(h => h.Name == eventMessage.Name)
                .SelectMany(h => h.Rooms)
                .Where(room => room.NumOfPeople >= totalGuests && room.RoomType == eventMessage.RoomType)
                .ToList();

            foreach (var room in suitableRooms) {
                Guid room_id = room.Id;
                var isAvailable = !_context.RoomEvent.Any(re => re.Status == "Reserved" && re.StartDate < eventMessage.ReturnDate.ToUniversalTime() && re.EndDate > eventMessage.ArrivalDate.ToUniversalTime() && re.RoomId == room_id);
            
                if (isAvailable) {
                    reserved = true;
                    var NewEvent = new RoomEvent {
                        Id = Guid.NewGuid(),
                        RoomId = room.Id,
                        Status = "Reserved",
                        StartDate = eventMessage.ArrivalDate.ToUniversalTime(),
                        EndDate = eventMessage.ReturnDate.ToUniversalTime()};
                    _context.RoomEvent.Add(NewEvent);
                    try {
                        _context.SaveChanges();
                        Console.WriteLine($"Reservation successful for Client ID: {eventMessage.ClientId}");
                        // publish na trip

                        await context.RespondAsync(new ReserveRoomReplyEvent {
                            Id = Guid.NewGuid(),
                            CorrelationId = eventMessage.CorrelationId,
                            SuccessfullyReserved = true
                        });
                    } catch (Exception e) {
                        Console.WriteLine("Reservation failed: " + e.Message);
                        await context.RespondAsync(new ReserveRoomReplyEvent {
                            Id = Guid.NewGuid(),
                            CorrelationId = eventMessage.CorrelationId,
                            SuccessfullyReserved = false
                        });
                    }
                    break;
                }
            }
            if (!reserved) {
                Console.WriteLine("No available rooms that meet the criteria.");
                await context.RespondAsync(new ReserveRoomReplyEvent {
                    Id = Guid.NewGuid(),
                    CorrelationId = eventMessage.CorrelationId,
                    SuccessfullyReserved = false
                });
            }
        }
    }
}
