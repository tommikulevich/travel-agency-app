using MassTransit;
using HotelService.Data;
using HotelService.Models;
using Shared.Hotel.Events;

namespace HotelService.Consumers {
    public class ReserveRoomEventConsumer : IConsumer<ReserveRoomEvent> {
        private readonly IHotelRepo _hotelRepository;
        private readonly HotelDbContext _context; 

        public ReserveRoomEventConsumer(IHotelRepo hotelRepository, HotelDbContext context) {
            _hotelRepository = hotelRepository;
            _context = context;
        }

        public async Task Consume(ConsumeContext<ReserveRoomEvent> context) {
            var hotelsRequest = new AvailableRoomsRequest {
                ArrivalPlace = context.Message.City,
                DepartureTime = context.Message.ArrivalDate,
                ArrivalTime = context.Message.ReturnDate
            };
            
            var availableHotels = _hotelRepository.GetAvailableHotels(hotelsRequest);

            foreach (var hotel in availableHotels) {
                var freeRooms = hotel.Rooms.Where(room => 
                    !room.RoomEvents.Any(re => re.Status == "Reserved" && re.StartDate < context.Message.ReturnDate && re.EndDate > context.Message.ArrivalDate)).ToList();

                if (freeRooms.Any()) {
                    foreach (var room in freeRooms) {
                        room.RoomEvents.Add(new RoomEvent {
                            Id = Guid.NewGuid(),
                            RoomId = room.Id,
                            Status = "Reserved",
                            StartDate = context.Message.ArrivalDate,
                            EndDate = context.Message.ReturnDate
                        });
                    }

                    try {
                        _context.SaveChanges();
                        Console.WriteLine($"Reservation successful for Client ID: {context.Message.ClientId}");

                        await context.RespondAsync(new ReserveRoomReplyEvent {
                            Id = Guid.NewGuid(), // We can replace with context.Message.Id if necessary
                            CorrelationId = context.Message.CorrelationId,
                            SuccessfullyReserved = true
                        });
                        return;
                    } catch (Exception e) {
                        Console.WriteLine("Reservation failed: " + e.Message);
                        await context.Publish(new ReserveRoomFailedEvent {
                            Id = Guid.NewGuid(), // We can replace with context.Message.Id if necessary
                            CorrelationId = context.Message.CorrelationId
                        });
                        return;
                    }
                }
            }

            Console.WriteLine("No available rooms that meet the criteria.");
            await context.Publish(new ReserveRoomFailedEvent {
                Id = Guid.NewGuid(), // We can replace with context.Message.Id if necessary
                CorrelationId = context.Message.CorrelationId
            });
        }
    }
}