namespace Shared.Flight.Dtos
{
    public class FlightDto
    {
        public Guid Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public string DeparturePlace {get; set;} = string.Empty;
        public string ArrivalPlace {get; set;} = string.Empty;
        public DateTime DepartureTime {get; set;}
        public DateTime ArrivalTime {get; set;}
        public int NumOfSeats {get; set;}
    }
}