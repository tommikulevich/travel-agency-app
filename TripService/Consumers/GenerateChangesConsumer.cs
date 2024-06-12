using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using Shared.Trip.Dtos;
using TripService.Models;
using Shared.Hotel.Events;
using Shared.Flight.Events;

namespace TripService.Consumers
{
    public class GenerateChangesConsumer : IConsumer<GenerateChangesEvent>
    {
        private readonly ITripRepo _tripRepo;
        
        public GenerateChangesConsumer(ITripRepo tripRepo)
        {
            _tripRepo = tripRepo;
        }

        public async Task Consume(ConsumeContext<GenerateChangesEvent> context)
        {
            Console.WriteLine("Data generating...");
            var changedTrip = _tripRepo.GetRandomTripAndGenerateChanges();
            if (changedTrip != null) 
            {
                Console.WriteLine("Generation successful!");
                await context.Publish(changedTrip);
                if (changedTrip.PreviousValue == "Zarezerwowana")
                {
                    Trip changedTripInRepo= _tripRepo.GetTripByGuid(changedTrip.OfferId);
                    Console.WriteLine("Unreserving room");
                    Guid corrId = Guid.NewGuid();
                    // context.Publish(new UnreserveRoomEvent{
                    //     CorrelationId = corrId,
                    //     ClientId = (Guid)changedTripInRepo.ClientId,
                    //     RoomId = changedTripInRepo.RoomId?????      // TODO unreserve without roomId or get room id somehow

                    // })
                    Console.WriteLine("Unreserving flight");
                    await context.Publish(new ReserveSeatsEvent{
                        CorrelationId = corrId,
                        FlightId = changedTripInRepo.FlightId,
                        Seats = (-1) * (changedTripInRepo.NumOfAdults + changedTripInRepo.NumOfKidsTo18 + changedTripInRepo.NumOfKidsTo10)
                    });
                    // context.Publish(context => context.Init<UnreserveRoomEvent>(
                    //         new UnreserveRoomEvent
                    //         {
                    //             CorrelationId = context.Saga.CorrelationId,
                    //             ClientId = context.Saga.ClientId,
                    //             RoomId = context.Saga.ReservedRoomId,
                    //             ArrivalDate = context.Saga.DepartureDate,
                    //             ReturnDate = context.Saga.ReturnDate
                    //         }))
                }
                
                

            } 
            else 
            {
                Console.WriteLine("Generation failed!");
            }

            await Task.Yield();     // Ensures that method runs asynchronously and avoids the warning
        }
    }
}