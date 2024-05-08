
namespace ReservationService.Events
{
    public class CreateReservationEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }

        public Guid ReservationId { get; set; }
        public Guid OfferId { get; set; }


    }

}