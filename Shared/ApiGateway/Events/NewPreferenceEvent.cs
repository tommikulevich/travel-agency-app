namespace Shared.ApiGateway.Events
{
    public class NewPreferenceEvent
    {
        public Guid CorrelationId { get; set; }

        public string newPreference {get; set;} = string.Empty;

        public string typeOfPreference {get;set;} = string.Empty;
    }
}