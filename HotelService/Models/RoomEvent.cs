namespace HotelService.Models {
    public class RoomEvent {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}