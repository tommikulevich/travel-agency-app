using Shared.Trip.Events;

namespace Shared.Hotel.Events {
    public class ReserveRoomReplyEvent {
        public Guid Id  { get; set; }
        public Guid CorrelationId  { get; set; }  

        public bool SuccessfullyReserved {get; set;}
    }
}