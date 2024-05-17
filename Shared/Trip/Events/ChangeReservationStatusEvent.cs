namespace Shared.Trip.Events
{
    public class ChangeReservationStatusEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }
        public Guid OfferId { get; set; } 
        public Guid? ClientId { get; set; }
        public string Status {get; set;} = string.Empty;  
    }
}