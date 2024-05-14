namespace Shared.Hotel.Events {
    public class RoomsAvailabilityAfterReservationEvent {
        public Guid Id  { get; set; }
        public Guid HotelId { get; set; }
        public int NumOfAdults {get; set;}
        public int NumOfKidsTo18 {get; set;}
        public int NumOfKidsTo10 {get; set;}
        public int NumOfKidsTo3 {get; set;}
        public DateTime ArrivalDate {get; set;}
        public DateTime ReturnDate {get; set;}
        public string RoomType {get; set;}
    }
}