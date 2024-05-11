using MassTransit;
using HotelService.Data;
using HotelService.Models;
using Shared.Hotel.Events;

namespace HotelService.Consumers
{
    public class ReserveRoomEventConsumer : IConsumer<ReserveRoomEvent>
    {
        private readonly IHotelRepo _hotelRepository;
        private readonly HotelDbContext _context; 

        public ReserveRoomEventConsumer(IHotelRepo hotelRepository, HotelDbContext context)
        {
            _hotelRepository = hotelRepository;
            _context = context;
        }

        public async Task Consume(ConsumeContext<ReserveRoomEvent> context)
        {
            var hotelsRequest = new AvailableRoomsRequest
            {
                ArrivalPlace = context.Message.City,
                DepartureTime = context.Message.ArrivalDate,
                ArrivalTime = context.Message.ReturnDate
            };
            
            var availableHotels = _hotelRepository.GetAvailableHotels(hotelsRequest);

            foreach (var hotel in availableHotels)
            {
                var selectedRooms = hotel.Rooms.Where(r => 
                    r.RoomType == context.Message.RoomType && 
                    r.NumOfPeople >= (context.Message.NumOfAdults + context.Message.NumOfKidsTo18 + context.Message.NumOfKidsTo10 + context.Message.NumOfKidsTo3))
                    .ToList();

                var freeRooms = selectedRooms.Where(room => 
                    !room.RoomEvents.Any(re => re.StartDate < context.Message.ReturnDate && re.EndDate > context.Message.ArrivalDate))
                    .ToList();

                if (freeRooms.Any())
                {
                    foreach (var room in freeRooms)
                    {
                        room.RoomEvents.Add(new RoomEvent {
                            RoomId = room.Id,
                            Status = "Reserved",
                            StartDate = context.Message.ArrivalDate,
                            EndDate = context.Message.ReturnDate
                        });
                    }

                    try
                    {
                        _context.SaveChanges();
                        Console.WriteLine($"Reservation successful for Client ID: {context.Message.ClientId}");

                        await context.RespondAsync(new ReserveRoomReplyEvent {
                            Id = Guid.NewGuid(),
                            CorrelationId = context.Message.CorrelationId
                        });
                        return;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Reservation failed: " + e.Message);
                        await context.Publish(new ReserveRoomFailedEvent {
                            Id = Guid.NewGuid(),
                            CorrelationId = context.Message.CorrelationId
                        });
                        return;
                    }
                }
            }

            Console.WriteLine("No available rooms that meet the criteria.");
            await context.Publish(new ReserveRoomFailedEvent {
                Id = Guid.NewGuid(),
                CorrelationId = context.Message.CorrelationId
            });
        }
    }
}