using TripService.Models;

namespace TripService.Data
{
    public class TripRepo : ITripRepo
    {
        private readonly AppDbContext _context;
        public TripRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateTrip(Trip offer)
        {
            if (offer == null) 
            {
                throw new ArgumentNullException(nameof(offer));
            }
            _context.Trip.Add(offer);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trip.ToList();
        }

        public IEnumerable<Trip> GetTripsByPreferences(string Destination, DateTime DepartureDate, 
                                                                        string DeparturePlace, int NumOfAdults, 
                                                                        int NumOfKidsTo18, int NumOfKidsTo10, 
                                                                        int NumOfKidsTo3)
        {
            IQueryable<Trip> query = _context.Trip;

            query = query.Where(t => t.Country == Destination);

            DateTime utcDepartureDate = DepartureDate.ToUniversalTime();
            query = query.Where(t => t.DepartureDate.Date == utcDepartureDate.Date);

            query = query.Where(t => t.DeparturePlace == DeparturePlace);

            query = query.Where(t => t.NumOfAdults == NumOfAdults);

            query = query.Where(t => t.NumOfKidsTo18 == NumOfKidsTo18);

            query = query.Where(t => t.NumOfKidsTo10 == NumOfKidsTo10);

            query = query.Where(t => t.NumOfKidsTo3 == NumOfKidsTo3);


            return query.ToList();
        }

        public List<Trip> GetTripById(string id)
        {
            Guid guid = Guid.Parse(id);
            return _context.Trip.Where(p => p.ClientId == guid).ToList();
        }

        public void SaveTrip(Trip Trip)
        {
            _context.Trip.Add(Trip);
            _context.SaveChanges();
        }

        public bool CheckAvailability(Guid TripId)
        {
            var trip = _context.Trip.FirstOrDefault(p => p.Id == TripId);
            string status = trip.Status;
            bool result;
            if (status == "Dostępna")
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public void ChangeReservationStatus(Guid TripId, string newReservationStatus, Guid? UserId)
        {
            // var trip = _context.Trip.FirstOrDefault(t => t.Id == TripId);
            // trip.Status = newReservationStatus;
            _context.Trip.FirstOrDefault(t => t.Id == TripId).ClientId = UserId;
            _context.Trip.FirstOrDefault(t => t.Id == TripId).Status = newReservationStatus;
            _context.SaveChanges();
        }

        public IEnumerable<Trip> GetTripsBySpecificRoomConfiguration(Guid HotelId, int NumOfAdults, 
                int NumOfKidsTo18, int NumOfKidsTo10, int NumOfKidsTo3, DateTime ArrivalDate,
                DateTime ReturnDate, string RoomType)
        {
            IQueryable<Trip> query = _context.Trip.Where(t => 
                t.HotelId == HotelId 
                && t.NumOfAdults == NumOfAdults
                && t.NumOfKidsTo18 == NumOfKidsTo18
                && t.NumOfKidsTo10 == NumOfKidsTo10
                && t.NumOfKidsTo3 == NumOfKidsTo3
                && t.DepartureDate == ArrivalDate.ToUniversalTime()
                && t.ReturnDate == ReturnDate.ToUniversalTime()
                && t.RoomType == RoomType
            );

            return query.ToList();
        }

        public IEnumerable<Trip> GetTripsByFlightId(Guid flightId)
        {
            IQueryable<Trip> query = _context.Trip.Where(p => 
                p.FlightId == flightId
            );
            
            return query.ToList();
        }
    }
}