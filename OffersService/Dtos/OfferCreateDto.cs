using System.ComponentModel.DataAnnotations;

namespace OffersServise.Dtos
{
    public class OfferCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Cost { get; set; }
    }
}