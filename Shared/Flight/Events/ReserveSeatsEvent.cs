
namespace Shared.Flight.Events
{
    public class ReserveSeatsEvent
    {
        public Guid Id  { get; set; }
        public Guid CorrelationId  { get; set; }
        public Guid? FlightId { get; set; }
        public int Seats { get; set; }

        // public ReserveSeatsEvent(Guid flightId, int seats)
        // {
        //     FlightId = flightId;
        //     Seats = seats;
        // }
    }
}
