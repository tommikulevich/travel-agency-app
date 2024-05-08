using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReservationService.Events;

namespace ReservationService.Models
{
    public class Reservation
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string offerId { get; set; }

        [Required]
        public string clientId { get; set; }

        [Required]
        public string flightId { get; set; }

        [Required]
        public double price { get; set; }

        [Required]
        public int numberOfPeaple { get; set; }

        public void SetFields(ReservationDto dto)
        {
                this.offerId = dto.offerId;
                this.clientId = dto.clientId;
                this.flightId = dto.flightId;
                this.price = dto.price;
                this.numberOfPeaple = dto.numberOfPeaple;
        }

        public ReservationDto ToReservationDto()
        {
            return new ReservationDto()
            {
                offerId = this.offerId,
                clientId = this.clientId,
                flightId = this.flightId,
                price = this.price,
                numberOfPeaple = this.numberOfPeaple
            };
        }
    }
}