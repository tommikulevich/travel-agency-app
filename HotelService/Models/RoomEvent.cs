namespace HotelService.Models {
    public class RoomEvent {
        public Guid Id { get; set; }
        public RoomEventStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public enum RoomEventStatus {
        Available,
        Reserved
    }
}