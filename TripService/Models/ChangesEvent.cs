using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripService.Models
{
    public class ChangesEvent
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid OfferId { get; set; }
        public string ChangeType {get; set;} = string.Empty;
        public string ChangeValue {get; set;} = string.Empty;
    }
}