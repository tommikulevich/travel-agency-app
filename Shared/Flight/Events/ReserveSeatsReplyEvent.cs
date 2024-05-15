
namespace Shared.Flight.Events
{
    public class ReserveSeatsReplyEvent
    {
        public Guid CorrelationId  { get; set; }
        public Guid? FlightId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public ReserveSeatsReplyEvent(Guid correlationId, Guid? flightId, bool success, string message)
        {
            CorrelationId = correlationId;
            FlightId = flightId;
            Success = success;
            Message = message;
        }
    }
}
