namespace Shared.Trip.Events
{
    public class CheckReservationStatusEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }
    }
}