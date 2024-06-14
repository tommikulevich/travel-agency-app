using FlightService.Data.Tables;

namespace FlightService.Repo
{
    public interface IFlightRepo
    {
        public IEnumerable<FlightEntity> GetAvailableFlights(string departurePlace, string arrivalPlace, 
                DateTime departureTime, DateTime arrivalTime, int freeSeats);
        public void ReserveSeats(Guid? id, int seats);
        public int GetNumOfFreeSeatsOfSpecificFlight(Guid? flightId);
        public IEnumerable<FlightEntity> GetAllFlights();
    }
}