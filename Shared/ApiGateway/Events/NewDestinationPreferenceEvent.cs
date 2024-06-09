namespace Shared.ApiGateway.Events
{
    public class NewDestinationPreferenceEvent
    {
        public Guid CorrelationId { get; set; }

        public string newPreference {get; set;}
    }
}