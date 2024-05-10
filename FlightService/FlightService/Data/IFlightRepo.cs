using FlightService.Data.Tables;

namespace FlightService.Repo
{
    public interface IFlightRepo
    {
        public IEnumerable<FlightEntity> GetAvailableFlights(string DeparturePlace, string ArrivalPlace, DateTime DepartureTime, DateTime ArrivalTime, int freeSeats);

        public void ReserveSeats(Guid Id, int seats); //czy tu na pewno void?

        public IEnumerable<FlightEntity> GetAllFlights();
    }


}