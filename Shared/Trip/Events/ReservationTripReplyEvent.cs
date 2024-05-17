namespace Shared.Trip.Events
{
    public class ReservationTripReplyEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }
    }
}