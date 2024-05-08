
using ReservationService.Models;

namespace ReservationService.Events
{
    public class GetReservationEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }

        public string UserId { get; set; }

    }

}