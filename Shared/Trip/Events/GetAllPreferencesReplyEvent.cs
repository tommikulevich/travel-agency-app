using Shared.Trip.Dtos;

namespace Shared.Trip.Events
{
    public class GetAllPreferencesReplyEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }

        public PreferencesDto Preferences {get;set;}
    }
}