namespace Shared.Trip.Events
{
    public class GetTripEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }

        public string UserId { get; set; }

    }

}