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
            _context.Trips.Add(offer);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trips.ToList();
        }

        public IEnumerable<Trip> GetTripsByPreferences(string Destination, DateTime DepartureDate, 
                                                                        string DeparturePlace, int NumOfAdults, 
                                                                        int NumOfKidsTo18, int NumOfKidsTo10, 
                                                                        int NumOfKidsTo3)
        {
            IQueryable<Trip> query = _context.Trips;

            query = query.Where(t => t.Country.ToLower().Contains(Destination.ToLower()));

            query = query.Where(t => t.DepartureDate.Date == DepartureDate.Date);

            query = query.Where(t => t.DeparturePlace.ToLower().Contains(DeparturePlace.ToLower()));

            query = query.Where(t => t.NumOfAdults == NumOfAdults);

            query = query.Where(t => t.NumOfKidsTo18 == NumOfKidsTo18);

            query = query.Where(t => t.NumOfKidsTo10 == NumOfKidsTo10);

            query = query.Where(t => t.NumOfKidsTo3 == NumOfKidsTo3);


            return query.ToList();
        }

        public List<Trip> GetTripById(string id)
        {
            return _context.Trips.Where(p => p.ClientId == id).ToList();
        }

        public void SaveTrip(Trip Trip)
        {
            _context.Trips.Add(Trip);
            _context.SaveChanges();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}