namespace Shared.Trip.Events
{
    public class ReservationTripEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid OfferId { get; set; }
        public Guid ClientId { get; set; }
        public Guid? FlightId { get; set; }
        public Guid HotelId { get; set; }
        public string Name {get; set;} = string.Empty;  
        public string Country {get; set;} = string.Empty;  
        public string? DeparturePlace {get; set;}
        public string? ArrivalPlace {get; set;}
        public int NumOfAdults {get; set;}
        public int NumOfKidsTo18 {get; set;}
        public int NumOfKidsTo10 {get; set;}
        public int NumOfKidsTo3 {get; set;}
        public DateTime DepartureDate {get; set;}
        public DateTime ReturnDate {get; set;}
        public string TransportType {get; set;} = string.Empty;      
        public double Price {get; set;}
        public string MealsType {get; set;} = string.Empty;        
        public string RoomType {get; set;} = string.Empty;          
        public double DiscountPercents {get; set;}
        public int NumOfNights {get; set;}  
        public string Status {get; set;} = string.Empty;    
    }
}