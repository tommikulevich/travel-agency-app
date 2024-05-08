
namespace PaymentService.Models
{
    public class EventModelClass
    {
        public Guid Id {get; set;}
        public Guid CorrelationID {get; set;}
        public DateTime CreationDate {get; private set;}
        public EventModelClass(Guid id, DateTime date, Guid correlationid)
        {
            Id = id;
            CreationDate = date;
            CorrelationID = correlationid;
        }

        public EventModelClass()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
            CorrelationID = Guid.NewGuid();
        }
    }
}