namespace Shared.Hotel.Events {
    public class UnreserveRoomEvent {
        public Guid CorrelationId { get; set; }
        public string ClientId { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}