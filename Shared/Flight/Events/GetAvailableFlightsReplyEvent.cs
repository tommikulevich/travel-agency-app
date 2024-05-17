using Shared.Flight.Dtos;

namespace Shared.Flight.Events
{
    public class GetAvailableFlightsReplyEvent
    {
        public Guid CorrelationId  { get; set; }
        public List<FlightDto> AvailableFlights { get; set; }

        public GetAvailableFlightsReplyEvent(List<FlightDto> availableFlights)
        {
            AvailableFlights = availableFlights;
        }
    }
}
