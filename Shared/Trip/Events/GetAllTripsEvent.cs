namespace Shared.Trip.Events
{
    public class GetAllTripsEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }

    }

}