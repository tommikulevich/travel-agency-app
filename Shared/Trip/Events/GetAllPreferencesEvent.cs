namespace Shared.Trip.Events
{
    public class GetAllPreferencesEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }
    }
}