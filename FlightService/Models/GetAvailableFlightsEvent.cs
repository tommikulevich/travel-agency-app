using System;

namespace FlightService.Events
{
    public class GetAvailableFlightsEvent
    {
        public string DeparturePlace { get; set; }
        public string ArrivalPlace { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int FreeSeats { get; set; }

        public GetAvailableFlightsEvent(string departurePlace, string arrivalPlace, DateTime departureTime, DateTime arrivalTime, int freeSeats)
        {
            DeparturePlace = departurePlace;
            ArrivalPlace = arrivalPlace;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            FreeSeats = freeSeats;
        }
    }
}
