namespace Shared.Hotel.Events {
    public class UnreserveRoomWithoutIdEvent {
        public Guid CorrelationId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime ReturnDate { get; set; }

        // Instead of RoomId
        public Guid HotelId { get; set; }
        public int NumOfPeople {get; set;}
        public string RoomType {get; set;} = string.Empty;
        public string Features  {get;set;} = string.Empty;
    }
}