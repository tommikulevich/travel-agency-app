namespace Shared.Hotel.Events {
    public class UnreserveRoomReplyEvent {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }  
        public bool Success { get; set; }
    }
}