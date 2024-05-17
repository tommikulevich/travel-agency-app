namespace HotelService.Models {
    public class Room {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public int NumOfPeople { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public string Features { get; set; } = string.Empty;
        public List<RoomEvent> RoomEvents { get; set; } = new List<RoomEvent>();
    }
}