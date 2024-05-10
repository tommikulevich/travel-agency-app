namespace HotelService.Models {
    public class Room {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public int NumOfPeople { get; set; }
        public int NumOfRooms { get; set; }
        public string RoomType { get; set; }
        public string Features { get; set; }
        public List<RoomEvent> RoomEvents { get; set; }
    }
}