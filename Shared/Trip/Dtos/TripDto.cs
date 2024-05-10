

namespace Shared.Trip.Dtos
{
    public class TripDto
    {
        public string OfferId { get; set; }
        public string ClientId { get; set; }
        public string FlightId { get; set; }
        public string Name {get; set;}
        public string Country {get; set;}
        public string DeparturePlace {get; set;}
        public int NumOfAdults {get; set;}
        public int NumOfKidsTo18 {get; set;}
        public int NumOfKidsTo10 {get; set;}
        public int NumOfKidsTo3 {get; set;}
        public DateTime DepartureDate {get; set;}
        public DateTime ReturnDate {get; set;}
        public string TransportType {get; set;}    
        public double Price {get; set;}
        public string MealsType {get; set;}
        public string RoomType {get; set;} 
        public double DiscountPercents {get; set;}
        public int NumOfNights {get; set;}    
        public string Status {get;set;}   


    }

    
}