using Shared.Trip.Events;

namespace Shared.Hotel.Events {
    public class ReserveRoomFailedEvent {
        public Guid Id  { get; set; }
        public Guid CorrelationId  { get; set; }  
    }
}