namespace Shared.Flight.Events
{
    public class ReserveSeatsFailedEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set;}
    }
}
