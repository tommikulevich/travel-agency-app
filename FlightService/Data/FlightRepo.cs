using FlightService.Data;
using FlightService.Data.Tables;

namespace FlightService.Repo
{
    public class FlightRepo : IFlightRepo
    {
        private readonly FlightContext _context;

        public FlightRepo(FlightContext context)
        {
            _context = context;
        }

        // public IEnumerable<FlightEntity> GetAvailableFlights(string departurePlace, string arrivalPlace, DateTime departureTime, DateTime arrivalTime, int freeSeats)
        // {
        //         return _context.Flights
        //         .Where(r => r.DeparturePlace == departurePlace && r.ArrivalPlace == arrivalPlace && r.DepartureTime >= departureTime && r.NumOfFreeSeats >= freeSeats) //event sourcing in here? 
        //         .Select(r => r)
        //         .ToList();
        // }

 public IEnumerable<FlightEntity> GetAvailableFlights(string departurePlace, string arrivalPlace, DateTime departureTime, DateTime arrivalTime, int freeSeats)
        {
            return _context.Flights
                .Where(r => r.DeparturePlace == departurePlace && r.ArrivalPlace == arrivalPlace && r.DepartureTime >= departureTime)
                .Where(r => r.NumOfSeats - _context.FlightSeatEvents.Where(e => e.Id == r.Id).Sum(e => e.ReservedSeats) >= freeSeats)   // FlightId changed to ID
                .ToList();
        }


       public void ReserveSeats(Guid Id, int seats)
        {
            var flight = _context.Flights.Find(Id);
            if (flight == null)
            {
                throw new Exception("Flight not found");
            }

            var currentFreeSeats = flight.NumOfSeats - _context.FlightSeatEvents.Where(e => e.Id == Id).Sum(e => e.ReservedSeats);  // FlightId changed to ID
            if (currentFreeSeats < seats)
            {
                throw new Exception("Not enough free seats");
            }

            _context.FlightSeatEvents.Add(new FlightSeatEvent
            {
                Id = Id,  // probably useless FlightId changed to ID 
                EventTime = DateTime.UtcNow,
                ReservedSeats = seats
            });

            _context.SaveChanges();
        }
        public IEnumerable<FlightEntity> GetAllFlights()
        {
             return _context.Flights
                .Where(r => r.DeparturePlace!=null)
                .Select(r => r)
                .ToList();
            
        }

    }




}