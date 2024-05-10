namespace Shared.Trip.Events
{
    public class GetTripsByUserIdEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }

        public string ClientId { get; set; }

    }

}