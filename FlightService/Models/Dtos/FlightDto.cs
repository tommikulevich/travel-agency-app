using System.ComponentModel.DataAnnotations;

namespace FlightService.Dto
{
    public class FlightDto
    {
        public Guid Id{get; set;}
        public string Name{get; set;}
        public string FlightID{get; set;}
         public string DeparturePlace{get; set;}
          public string ArrivalPlace{get; set;}
          public DateTime DepartureTime{get; set;}
          public  DateTime ArrivalTime{get; set;}
          public int NumOfSeats{get; set;}
           public int NumOfFreeSeats{get; set;}

    }
}