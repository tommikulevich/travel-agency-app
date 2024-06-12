using MassTransit;
using TripService.Data;
using Shared.Trip.Events;

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
            } 
            else 
            {
                Console.WriteLine("Generation failed!");
            }

            await Task.Yield();     // Ensures that method runs asynchronously and avoids the warning
        }
    }
}