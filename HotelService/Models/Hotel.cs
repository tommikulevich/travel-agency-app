namespace HotelService.Models {
    public class Hotel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string AirportName { get; set; }
        public List<Room> Rooms { get; set; }
    }
}