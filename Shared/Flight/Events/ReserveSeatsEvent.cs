
namespace Shared.Flight.Events
{
    public class ReserveSeatsEvent
    {
        public Guid FlightId { get; set; }
        public int Seats { get; set; }

        public ReserveSeatsEvent(Guid flightId, int seats)
        {
            FlightId = flightId;
            Seats = seats;
        }
    }
}
