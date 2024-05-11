namespace Shared.Trip.Events
{
    public class ReservationTimeoutEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }

    }
}