namespace Shared.Trip.Events
{
    public class ReservationTimeoutEvent
    {
        public Guid CorrelationId  { get; set; }
    }
}