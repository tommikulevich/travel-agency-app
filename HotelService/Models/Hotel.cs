namespace HotelService.Models {
    public class Hotel {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string AirportName { get; set; } = string.Empty;
        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}