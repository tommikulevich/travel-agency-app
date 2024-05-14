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
            var eventMessage = context.Message;
            var totalGuests = eventMessage.NumOfAdults + eventMessage.NumOfKidsTo18 
                + eventMessage.NumOfKidsTo10 + eventMessage.NumOfKidsTo3;

            var suitableRooms = _context.Hotel
                .Where(h => h.Name == eventMessage.Name)
                .SelectMany(h => h.Rooms)
                .Where(room => room.NumOfPeople >= totalGuests 
                    && room.RoomType == eventMessage.RoomType)
                .ToList();

            int numOfAvailableRooms = 0;
            var zeros = new Byte[16];
            Guid avaliableRoomId = new Guid(zeros);
            Guid avaliableHotelId = new Guid(zeros);
            
            foreach (var room in suitableRooms) {
                Guid roomId = room.Id;
                var isAvailable = !_context.RoomEvent.Any(re => re.Status == "Reserved" 
                    && re.StartDate < eventMessage.ReturnDate.ToUniversalTime() 
                    && re.EndDate > eventMessage.ArrivalDate.ToUniversalTime() 
                    && re.RoomId == roomId);

                if (isAvailable) {
                    avaliableRoomId = roomId;
                    avaliableHotelId = room.HotelId;
                    numOfAvailableRooms += 1;
                }
            }
            
            if (numOfAvailableRooms > 0) {
                var NewEvent = new RoomEvent {
                    Id = Guid.NewGuid(),
                    RoomId = avaliableRoomId,
                    Status = "Reserved",
                    StartDate = eventMessage.ArrivalDate.ToUniversalTime(),
                    EndDate = eventMessage.ReturnDate.ToUniversalTime()
                };
                _context.RoomEvent.Add(NewEvent);

                try {
                    _context.SaveChanges();
                    Console.WriteLine($"Reservation successful for Client ID: {eventMessage.ClientId}");

                    await context.RespondAsync(new ReserveRoomReplyEvent {
                        Id = Guid.NewGuid(),
                        CorrelationId = eventMessage.CorrelationId,
                        SuccessfullyReserved = true
                    });

                    if (numOfAvailableRooms > 1) {
                        await context.Publish(new RoomsAvailabilityAfterReservationEvent {
                            Id = Guid.NewGuid(),
                            HotelId = avaliableHotelId,
                            NumOfAdults = eventMessage.NumOfAdults,
                            NumOfKidsTo18 = eventMessage.NumOfKidsTo18,
                            NumOfKidsTo10 = eventMessage.NumOfKidsTo10,
                            NumOfKidsTo3 = eventMessage.NumOfKidsTo3,
                            ArrivalDate = eventMessage.ArrivalDate,
                            ReturnDate = eventMessage.ReturnDate,
                            RoomType = eventMessage.RoomType  
                        });
                    }
                } catch (Exception e) {
                    Console.WriteLine("Reservation failed: " + e.Message);
                    await context.RespondAsync(new ReserveRoomReplyEvent {
                        Id = Guid.NewGuid(),
                        CorrelationId = eventMessage.CorrelationId,
                        SuccessfullyReserved = false
                    });
                }
            } else {
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
