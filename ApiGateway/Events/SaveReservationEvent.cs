
using ReservationService.Models;

namespace ReservationService.Events
{
    public class SaveReservationEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }
        public Reservation reservation {get; set;}

    }

}