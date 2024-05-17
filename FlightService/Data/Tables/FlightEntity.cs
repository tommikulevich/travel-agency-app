using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Flight.Dtos;

namespace FlightService.Data.Tables
{
    public class FlightEntity
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name {get; set;} = string.Empty;
        public string DeparturePlace {get; set;} = string.Empty;
        public string ArrivalPlace {get; set;} = string.Empty;
        public DateTime DepartureTime {get; set;}
        public  DateTime ArrivalTime {get; set;}
        public int NumOfSeats {get; set;}

        public void SetFields(FlightDto dto)
        {
            this.Name=dto.Name;
            this.DeparturePlace = dto.DeparturePlace;
            this.ArrivalPlace = dto.ArrivalPlace;
            this.DepartureTime=dto.DepartureTime;
            this.ArrivalTime = dto.ArrivalTime;
            this.NumOfSeats = dto.NumOfSeats;
        }
        
        public FlightDto ToFlightDto()
        {
            return new FlightDto()
            {
                Name= this.Name,
                DeparturePlace = this.DeparturePlace,
                ArrivalPlace =  this.ArrivalPlace,
                DepartureTime= this.DepartureTime,
                ArrivalTime =  this.ArrivalTime,
                NumOfSeats =  this.NumOfSeats,
            };
        }
    }
}