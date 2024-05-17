using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class FlightSeatEvent
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid? FlightId { get; set; }
    public int ReservedSeats { get; set; }
}