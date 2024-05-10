using Shared.Trip.Events;

namespace Shared.Hotel.Events {
    public class ReserveRoomEvent {
        public Guid Id  { get; set; }
        public Guid CorrelationId  { get; set; }
        // public Guid RoomId { get; set; }
        public string ClientId { get; set; }
        public string Name {get; set;}
        public string Country {get; set;}
        public string City {get; set;}
        public int NumOfAdults {get; set;}
        public int NumOfKidsTo18 {get; set;}
        public int NumOfKidsTo10 {get; set;}
        public int NumOfKidsTo3 {get; set;}
        public DateTime ArrivalDate {get; set;}
        public DateTime ReturnDate {get; set;}
        public string RoomType {get; set;}    
        public int NumOfNights {get; set;}   
    }
}