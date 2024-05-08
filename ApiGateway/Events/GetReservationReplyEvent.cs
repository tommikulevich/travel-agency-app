
using ReservationService.Models;

namespace ReservationService.Events
{
    public class GetReservationReplyEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId  { get; set; }

        public IEnumerable<ReservationDto> Reservations { get; set; }

    }

}