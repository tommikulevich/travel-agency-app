namespace Shared.Hotel.Events {
    public class ReserveRoomEvent {
        public Guid Id  { get; set; }
        public Guid CorrelationId  { get; set; }
        public Guid ClientId { get; set; }
        public string Name {get; set;} = string.Empty;
        public string Country {get; set;} = string.Empty;
        public string City {get; set;} = string.Empty;
        public int NumOfAdults {get; set;}
        public int NumOfKidsTo18 {get; set;}
        public int NumOfKidsTo10 {get; set;}
        public int NumOfKidsTo3 {get; set;}
        public DateTime ArrivalDate {get; set;}
        public DateTime ReturnDate {get; set;}
        public string RoomType {get; set;} = string.Empty;    
        public int NumOfNights {get; set;}   
    }
}