namespace Shared.Hotel.Events {
    public class UnreserveRoomEvent {
        public Guid CorrelationId { get; set; }
        public Guid ClientId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}