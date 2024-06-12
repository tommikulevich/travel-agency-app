using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using MassTransit.SqlTransport.Topology;
using Shared.ApiGateway.Events;

namespace TripService.Consumers
{
    public class ChangeReservationStatusConsumer : IConsumer<ChangeReservationStatusEvent>
    {
        private readonly ITripRepo _tripRepo;
        
        public ChangeReservationStatusConsumer(ITripRepo tripRepo)
        {
            _tripRepo = tripRepo;
        }

        public async Task Consume(ConsumeContext<ChangeReservationStatusEvent> context)
        {
            Console.WriteLine("Reservation status changed");
            
            Guid? ClientId = context.Message.ClientId;
            var offerId = context.Message.OfferId;
            var status = context.Message.Status;

            string previous_top_destination = "";
            string new_top_destination = "";

            string previous_top_hotel = "";
            string new_top_hotel = "";

            string previous_top_room = "";
            string new_top_room = "";

            string previous_top_transport = "";
            string new_top_transport = "";

            if (status == "Zarezerwowana")
            {
                previous_top_destination = _tripRepo.GetMostPopularReservedDestination();
                previous_top_hotel = _tripRepo.GetMostPopularReservedHotel();
                previous_top_room = _tripRepo.GetMostPopularReservedRoom();
                previous_top_transport = _tripRepo.GetMostPopularReservedTransport();
            }
            
            
            _tripRepo.ChangeReservationStatus(offerId, status, ClientId);

            if (status == "Zarezerwowana")
            {
                new_top_destination = _tripRepo.GetMostPopularReservedDestination();
                new_top_hotel = _tripRepo.GetMostPopularReservedHotel();
                new_top_room = _tripRepo.GetMostPopularReservedRoom();
                new_top_transport = _tripRepo.GetMostPopularReservedTransport();
                // Destination preference changed
                if (new_top_destination != previous_top_destination)
                {
                    await context.Publish(new NewPreferenceEvent() {
                    CorrelationId = Guid.NewGuid(), 
                    newPreference = new_top_destination,
                    typeOfPreference = "destination"
                    });
                    Console.WriteLine("Dest Preference changed");
                    Console.WriteLine(new_top_destination);
                    Console.WriteLine(previous_top_destination);
                }
                else
                {
                    Console.WriteLine("Dest Preference not changed");
                    Console.WriteLine(new_top_destination);
                    Console.WriteLine(previous_top_destination);
                }

                // Hotel preference changed
                if (new_top_hotel != previous_top_hotel)
                {
                    await context.Publish(new NewPreferenceEvent() {
                    CorrelationId = Guid.NewGuid(), 
                    newPreference = new_top_hotel,
                    typeOfPreference = "hotel"
                    });
                    Console.WriteLine("Hotel Preference changed");
                    Console.WriteLine(new_top_hotel);
                    Console.WriteLine(previous_top_hotel);
                }
                else
                {
                    Console.WriteLine("Hotel Preference not changed");
                    Console.WriteLine(new_top_hotel);
                    Console.WriteLine(previous_top_hotel);
                }

                // Room preference changed
                if (new_top_room != previous_top_room)
                {
                    await context.Publish(new NewPreferenceEvent() {
                    CorrelationId = Guid.NewGuid(), 
                    newPreference = new_top_room,
                    typeOfPreference = "room"
                    });
                    Console.WriteLine("Room Preference changed");
                    Console.WriteLine(new_top_room);
                    Console.WriteLine(previous_top_room);
                }
                else
                {
                    Console.WriteLine("Room Preference not changed");
                    Console.WriteLine(new_top_room);
                    Console.WriteLine(previous_top_room);
                }

                // Transport preference changed
                if (new_top_transport != previous_top_transport)
                {
                    await context.Publish(new NewPreferenceEvent() {
                    CorrelationId = Guid.NewGuid(), 
                    newPreference = new_top_transport,
                    typeOfPreference = "transport"
                    });
                    Console.WriteLine("Room Preference changed");
                    Console.WriteLine(new_top_transport);
                    Console.WriteLine(previous_top_transport);
                }
                else
                {
                    Console.WriteLine("Room Preference not changed");
                    Console.WriteLine(new_top_transport);
                    Console.WriteLine(previous_top_transport);
                }
            }

            await Task.Yield();     // Ensures that method runs asynchronously and avoids the warning
        }
    }
}