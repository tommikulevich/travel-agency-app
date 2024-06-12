using System.Dynamic;
using Shared.Trip.Events;

namespace Shared.Trip.Dtos
{
    public class ChangesEventDto
    {
        public Guid CorrelationId {get; set;}
        public string destinationPreference {get; set;}
        public Guid OfferId { get; set; }
        public string ChangeType {get; set;} = string.Empty;
        public string ChangeValue {get; set;} = string.Empty;
        public string PreviousValue {get;set;}
    }
}