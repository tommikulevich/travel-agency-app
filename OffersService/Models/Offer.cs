using System.ComponentModel.DataAnnotations;

namespace OffersServise.Models
{
    public class Offer
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Cost { get; set; }
    }
}