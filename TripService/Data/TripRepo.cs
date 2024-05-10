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