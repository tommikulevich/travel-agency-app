namespace Shared.Trip.Events
{
    public class CheckReservationStatusReplyEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public bool available {get; set;}
    }
}