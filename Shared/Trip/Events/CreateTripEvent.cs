
namespace Shared.Trip.Events
{
    public class CreateTripEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }

        public Guid TripId { get; set; }
        public Guid OfferId { get; set; }


    }

}