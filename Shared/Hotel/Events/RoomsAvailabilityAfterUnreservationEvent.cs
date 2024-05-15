namespace Shared.Hotel.Events {
    public class RoomsAvailabilityAfterUnreservationEvent {
        public Guid Id  { get; set; }
        public Guid CorrelationId { get; set; }  
    }
}