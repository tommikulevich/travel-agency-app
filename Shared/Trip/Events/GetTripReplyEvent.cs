using Shared.Trip.Dtos;

namespace Shared.Trip.Events
{
    public class GetTripReplyEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }

        public IEnumerable<TripDto> Trips { get; set; }

    }

}