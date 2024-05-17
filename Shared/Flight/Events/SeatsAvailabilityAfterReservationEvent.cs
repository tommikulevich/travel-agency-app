namespace Shared.Flight.Events {
    public class SeatsAvailabilityAfterReservationEvent {
        public Guid Id { get; set; }
        public Guid? FlightId { get; set; }
        public int NumOfFreeSeats { get; set; }
    }
}